using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nfecreator
{
    class Clientes
    {

        int id;
        int codigo;
        String nome;
        String razao;
        String tipoe;
        String endereco;
        String complemento;
        String comple2;
        String bairro;
        String cidade;
        String codcidade;
        String uf;
        String coduf;
        String pais;
        String codpais;
        String cep;
        String fone_res;
        String fone2;
        String estado_c;
        String cpf;
        String rg;
        String rgpfisica;
        String suframa;
        String fone_trab;
        String obs;
        String nascimento;
        String cadastro;
        String email;
        String campo80;
        String exportado;
        String importado;
        String docobs;
        String consumidor;
        String contrib;
        String finan;
        string ins_mun;
        string id_estrangeiro;

        public Clientes() { }
     

        public int Id { get => id; set => id = value; }
        public int Codigo { get => codigo; set => codigo = value; }
        [JsonProperty(PropertyName = "fantasia")]
        public string Nome { get => nome; set => nome = value; }
        [JsonProperty(PropertyName = "nome")]
        public string Razao { get => razao; set => razao = value; }
        public string Tipoe { get => tipoe; set => tipoe = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        [JsonProperty(PropertyName = "numero")]
        public string Complemento { get => complemento; set => complemento = value; }
        [JsonProperty(PropertyName = "Complemento")]
        public string Comple2 { get => comple2; set => comple2 = value; }
        public string Bairro { get => bairro; set => bairro = value; }
        public string Cidade { get => cidade; set => cidade = value; }
        public string Codcidade { get => codcidade; set => codcidade = value; }
        public string Uf { get => uf; set => uf = value; }
        public string Coduf { get => coduf; set => coduf = value; }
        public string Pais { get => pais; set => pais = value; }
        public string Codpais { get => codpais; set => codpais = value; }
        [JsonProperty(PropertyName = "cep")]
        public string Cep { get => cep; set => cep = value; }
        [JsonProperty(PropertyName = "telefone")]
        public string Fone_res { get => fone_res; set => fone_res = value; }
        public string Fone2 { get => fone2; set => fone2 = value; }
        public string Estado_c { get => estado_c; set => estado_c = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Rg { get => rg; set => rg = value; }
        public string Rgpfisica { get => rgpfisica; set => rgpfisica = value; }
        public string Suframa { get => suframa; set => suframa = value; }
        public string Fone_trab { get => fone_trab; set => fone_trab = value; }
        public string Obs { get => obs; set => obs = value; }
        public string Nascimento { get => nascimento; set => nascimento = value; }
        public string Cadastro { get => cadastro; set => cadastro = value; }
        public string Email { get => email; set => email = value; }
        public string Campo80 { get => campo80; set => campo80 = value; }
        public string Exportado { get => exportado; set => exportado = value; }
        public string Importado { get => importado; set => importado = value; }
        public string Docobs { get => docobs; set => docobs = value; }
        public string Consumidor { get => consumidor; set => consumidor = value; }
        public string Contrib { get => contrib; set => contrib = value; }
        public string Finan { get => finan; set => finan = value; }
        public int Id1 { get => id; set => id = value; }
        public string CodigoNome { get => Codigo + " - " + Nome; }
        public string Ins_mun { get => ins_mun; set => ins_mun = value; }
        public string Id_estrangeiro { get => id_estrangeiro; set => id_estrangeiro = value; }

        public Clientes(string codigo)
        {

            try
            {
                DbfBase ebase = new DbfBase();
                string instrucao = @"SELECT codigo, nome, razao, contrib, email from " + ebase.Path + @"\CLIENTES.dbf WHERE codigo = '" + codigo + "' ";

                OleDbCommand cmd = new OleDbCommand(instrucao, ebase.Conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                DataTable dt = ds.Tables[0];
                ebase.Close();

                foreach (DataRow row in dt.Rows)
                {
                    codigo = row["codigo"].ToString().Trim();
                    nome = row["nome"].ToString().Trim();
                    razao = row["razao"].ToString().Trim();

                    contrib = row["contrib"].ToString().Trim();

                    email = row["email"].ToString().Trim();

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot open file"))
                {
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF - TABELA CLIENTE", true);
                }
                else
                    Funcoes.Crashe(ex, "TABELA ABERTA NO CIAF- TABELA CLIENTE", true);

            }

        }


    }
}
