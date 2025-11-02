using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Security;
using System.Xml;
using System.Xml.Schema;

namespace nfecreator.Validation
{
    /// <summary>
    /// Validates NFe XML against a specified root XSD, resolving includes/imports strictly from a base folder.
    /// Also logs which XSD files/URIs were resolved during validation.
    /// </summary>
    public static class NFeSchemaValidator
    {
        public class ValidationResult
        {
            public bool Success { get; set; }
            public string Log { get; set; }
            public List<string> LoadedSchemas { get; } = new List<string>();
            public List<string> Errors { get; } = new List<string>();
        }

        /// <summary>
        /// Validate an XML string against a root XSD, forcing all schema resolution to the specified folder.
        /// </summary>
        /// <param name="xmlContent">XML content to validate.</param>
        /// <param name="rootXsdPath">Absolute path to the root XSD (e.g., C:\\Schemas\\NFe4\\enviNFe_v4.00.xsd).</param>
        /// <param name="schemasBaseFolder">Base folder where all XSDs must be resolved from. If null, takes Path.GetDirectoryName(rootXsdPath).</param>
        /// <returns>ValidationResult with success flag, errors, and a log including resolved schemas.</returns>
        public static ValidationResult ValidateXmlString(string xmlContent, string rootXsdPath, string schemasBaseFolder = null)
        {
            var result = new ValidationResult();
            var log = new StringBuilder();

            try
            {
                if (string.IsNullOrWhiteSpace(xmlContent))
                    throw new ArgumentException("XML content is empty.");
                if (string.IsNullOrWhiteSpace(rootXsdPath))
                    throw new ArgumentException("Root XSD path is empty.");

                rootXsdPath = Path.GetFullPath(rootXsdPath);
                if (!File.Exists(rootXsdPath))
                    throw new FileNotFoundException("Root XSD not found", rootXsdPath);

                schemasBaseFolder = string.IsNullOrWhiteSpace(schemasBaseFolder)
                    ? Path.GetDirectoryName(rootXsdPath)
                    : schemasBaseFolder;

                if (string.IsNullOrWhiteSpace(schemasBaseFolder) || !Directory.Exists(schemasBaseFolder))
                    throw new DirectoryNotFoundException($"Schemas base folder not found: {schemasBaseFolder}");

                log.AppendLine($"[SchemaValidation] RootXsd: {rootXsdPath}");
                log.AppendLine($"[SchemaValidation] BaseFolder: {schemasBaseFolder}");

                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    DtdProcessing = DtdProcessing.Prohibit
                };

                var resolver = new RestrictedFolderXmlUrlResolver(schemasBaseFolder, result.LoadedSchemas);
                settings.Schemas.XmlResolver = resolver;
                settings.XmlResolver = resolver;

                // Load the root schema explicitly into the schema set, so that includes/imports are followed via resolver
                using (var schemaReader = XmlReader.Create(rootXsdPath, new XmlReaderSettings { XmlResolver = resolver }))
                {
                    settings.Schemas.Add(null, schemaReader);
                }

                settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings |
                                           XmlSchemaValidationFlags.ProcessSchemaLocation |
                                           XmlSchemaValidationFlags.ProcessInlineSchema;

                settings.ValidationEventHandler += (sender, args) =>
                {
                    var msg = $"[{args.Severity}] {args.Message} (Line {args.Exception?.LineNumber}, Pos {args.Exception?.LinePosition})";
                    result.Errors.Add(msg);
                    log.AppendLine(msg);
                };

                using (var sr = new StringReader(xmlContent))
                using (var reader = XmlReader.Create(sr, settings))
                {
                    while (reader.Read()) { /* iterate to trigger validation */ }
                }

                // Build log of loaded schemas
                if (result.LoadedSchemas.Count > 0)
                {
                    log.AppendLine("[SchemaValidation] Loaded XSDs:");
                    foreach (var s in result.LoadedSchemas)
                        log.AppendLine(" - " + s);
                }

                result.Success = result.Errors.Count == 0;
                if (result.Success)
                    log.AppendLine("[SchemaValidation] SUCCESS");
                else
                    log.AppendLine("[SchemaValidation] FAILED");
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.ToString());
                log.AppendLine("[SchemaValidation] EXCEPTION: " + ex);
                result.Success = false;
            }

            result.Log = log.ToString();
            return result;
        }

        /// <summary>
        /// XmlUrlResolver that resolves only inside a given base folder, logging resolved URIs.
        /// </summary>
        private class RestrictedFolderXmlUrlResolver : XmlUrlResolver
        {
            private readonly string baseFolder;
            private readonly ISet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            private readonly List<string> loadLog;

            public RestrictedFolderXmlUrlResolver(string baseFolder, List<string> loadLog)
            {
                this.baseFolder = Path.GetFullPath(baseFolder);
                this.loadLog = loadLog ?? new List<string>();
            }

            public override Uri ResolveUri(Uri baseUri, string relativeUri)
            {
                // Combine base folder with the provided relative path or filename
                string candidatePath;
                if (Uri.TryCreate(relativeUri, UriKind.Absolute, out var abs))
                {
                    // If an absolute URI is provided, map it to a file within baseFolder by using only filename
                    candidatePath = Path.Combine(baseFolder, Path.GetFileName(abs.LocalPath));
                }
                else if (!string.IsNullOrEmpty(baseUri?.LocalPath))
                {
                    candidatePath = Path.Combine(baseFolder, Path.GetFileName(relativeUri));
                }
                else
                {
                    candidatePath = Path.Combine(baseFolder, relativeUri ?? string.Empty);
                }

                var full = Path.GetFullPath(candidatePath);
                // Ensure the resolved path remains within baseFolder
                if (!full.StartsWith(this.baseFolder, StringComparison.OrdinalIgnoreCase))
                    throw new SecurityException($"Attempt to resolve schema outside base folder: {full}");

                return new Uri(full);
            }

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                var full = absoluteUri.IsFile ? absoluteUri.LocalPath : absoluteUri.ToString();
                if (!File.Exists(full))
                {
                    // Try fallback by filename within base folder
                    var fallback = Path.Combine(baseFolder, Path.GetFileName(full));
                    if (File.Exists(fallback))
                        full = Path.GetFullPath(fallback);
                }

                if (!File.Exists(full))
                    throw new FileNotFoundException("Schema not found during resolution", full);

                if (seen.Add(full))
                {
                    loadLog.Add(full);
                }

                return File.OpenRead(full);
            }
        }
    }
}
