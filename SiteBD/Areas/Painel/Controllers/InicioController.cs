using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteBD.Areas.Painel.Models;
using System.Data.SqlClient;

namespace SiteBD.Areas.Painel.Controllers
{
    public class InicioController : Controller
    {
        //
        // GET: /Painel/Inicio/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Pessoa pessoa)
        {
            if (string.IsNullOrEmpty(pessoa.Nome)) {
                return View();
            }
            if(pessoa.Nome.Equals("marcos") && pessoa.Senha.Equals("123"))
            {
               return RedirectToAction("PainelControle", pessoa);
            }
            return View();
        }
        public ActionResult PainelControle(Pessoa pessoa)

        {
            ViewBag.nome = pessoa.Nome;

            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            var strQuery = "select e.idEmpresa, e.descricao , e.telefone ,e.endereco ,e.idarea,a.Area from Empresa e ,Area a where e.idArea = a.idArea";
            var comando = new SqlCommand(strQuery, minhaconexao);
            var retorno = comando.ExecuteReader();

            var listaDeEmpresa = new List<Empresa>();


            while (retorno.Read())
            {
                var tempEmpresa = new Empresa();
                tempEmpresa.idEmpresa = int.Parse(retorno["idEmpresa"].ToString());
                tempEmpresa.Descricao = retorno["Descricao"].ToString();
                tempEmpresa.Telefone = retorno["Telefone"].ToString();
                tempEmpresa.Endereco = retorno["Endereco"].ToString();
                tempEmpresa.idArea = int.Parse(retorno["idArea"].ToString());
                tempEmpresa.Area = retorno["Area"].ToString();

                listaDeEmpresa.Add(tempEmpresa);
            }

            return View(listaDeEmpresa);
        }
        public ActionResult Detalhe(int id)
        {
            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            var strQuery = string.Format("SELECT * FROM EMPRESA WHERE IDEMPRESA = {0}", id);

            var comando = new SqlCommand(strQuery, minhaconexao);
            var retorno = comando.ExecuteReader();


            var listaDeEmpresa = new List<Empresa>();

            while (retorno.Read())
            {
                var tempEmpresa = new Empresa();
                tempEmpresa.idEmpresa = int.Parse(retorno["idEmpresa"].ToString());
                tempEmpresa.Descricao = retorno["Descricao"].ToString();
                tempEmpresa.Telefone = retorno["Telefone"].ToString();
                tempEmpresa.Endereco = retorno["Endereco"].ToString();
                tempEmpresa.idArea = int.Parse(retorno["IdArea"].ToString());
                listaDeEmpresa.Add(tempEmpresa);
            }


            return View(listaDeEmpresa.FirstOrDefault());
        }
        public ActionResult Excluir(int id) 
        {
            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            var strQuery = string.Format("DELETE FROM EMPRESA WHERE IDEMPRESA = {0}", id);

            var comando = new SqlCommand(strQuery, minhaconexao);
            var retorno = comando.ExecuteReader();

            return RedirectToAction("PainelControle");
        }
        public ActionResult Cadastrar()
        {
            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaConexao = new SqlConnection(stringConect);
            minhaConexao.Open();

            var strQuery = "select * from Area";
            var comando = new SqlCommand(strQuery, minhaConexao);
            var retorno = comando.ExecuteReader();


            var listaDeAreas = new List<Area>();

            while (retorno.Read())
            {
                var tempArea = new Area();
                tempArea.Id = int.Parse(retorno["idArea"].ToString());
                tempArea.Nome = retorno["Area"].ToString();
                listaDeAreas.Add(tempArea);
            }

            ViewBag.listaDeAreas = listaDeAreas;
            return View(new Empresa());
        }
        [HttpPost]
        public ActionResult Cadastrar(Empresa empresa)
        {

            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            if(ModelState.IsValid)
            {

                var strQuery = string.Format("INSERT INTO EMPRESA (Descricao,Telefone,Endereco,idArea) VALUES('{0}','{1}','{2}','{3}')",empresa.Descricao,empresa.Telefone,empresa.Endereco,empresa.idArea);

                var comando = new SqlCommand(strQuery, minhaconexao);
                var retorno = comando.ExecuteNonQuery();

                return RedirectToAction("PainelControle");
            }
            #region Executa comando da raca


            var strQueryArea = "SELECT * FROM Area";
            var comandoArea = new SqlCommand(strQueryArea, minhaconexao);
            var retornoArea = comandoArea.ExecuteReader();
            #endregion

            #region Processa o retorno do select da raca
            //TODO: 3º Processar o retorno do banco de dados OU não

            var listaDeAreas = new List<Area>();

            while (retornoArea.Read())
            {
                var tempArea = new Area();
                tempArea.Id = int.Parse(retornoArea["idArea"].ToString());
                tempArea.Nome = retornoArea["Area"].ToString();
                listaDeAreas.Add(tempArea);
            }

            ViewBag.ListaDeAreas = listaDeAreas;
            #endregion
            return View(empresa);
        }
        public ActionResult Editar(int id)
        {
            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            var strQuery = string.Format("SELECT * FROM EMPRESA WHERE IDEMPRESA = {0}", id);

            var comando = new SqlCommand(strQuery, minhaconexao);
            var retorno = comando.ExecuteReader();


            var listaDeEmpresa = new List<Empresa>();

            while (retorno.Read())
            {
                var tempEmpresa = new Empresa();
                tempEmpresa.idEmpresa = int.Parse(retorno["idEmpresa"].ToString());
                tempEmpresa.Descricao = retorno["Descricao"].ToString();
                tempEmpresa.Telefone = retorno["Telefone"].ToString();
                tempEmpresa.Endereco = retorno["Endereco"].ToString();

                listaDeEmpresa.Add(tempEmpresa);
            }
            retorno.Close();

            #region Executa comando da raca
            var strQueryArea = "SELECT * FROM Area";
            var comandoArea = new SqlCommand(strQueryArea, minhaconexao);
            var retornoArea = comandoArea.ExecuteReader();
            #endregion

            #region Processa o retorno do select da raca
            //TODO: 3º Processar o retorno do banco de dados OU não

            var listaDeAreas = new List<Area>();

            while (retornoArea.Read())
            {
                var tempArea = new Area();
                tempArea.Id = int.Parse(retornoArea["idArea"].ToString());
                tempArea.Nome = retornoArea["Area"].ToString();
                listaDeAreas.Add(tempArea);
            }

            ViewBag.ListaDeAreas = listaDeAreas;
            #endregion


            return View(listaDeEmpresa.FirstOrDefault());
           
        }
        [HttpPost]
        public ActionResult Editar(Empresa empresa)
        {
            const string stringConect = @"data source=VINICIOS;Integrated Security=SSPI;Initial Catalog=ChuckNorris";
            var minhaconexao = new SqlConnection(stringConect);
            minhaconexao.Open();

            if (ModelState.IsValid)
            {        
                var strQuery = string.Format("UPDATE EMPRESA SET Descricao = '{0}',Telefone = '{1}', Endereco = '{2}',idArea ='{3}'  WHERE IDEMPRESA ='{4}'", empresa.Descricao, empresa.Telefone, empresa.Endereco,empresa.idArea, empresa.idEmpresa);

                var comando = new SqlCommand(strQuery, minhaconexao);
                var retorno = comando.ExecuteReader();
                return RedirectToAction("PainelControle");
            }
            #region Executa comando da area
            var strQueryArea = "SELECT * FROM Area";
            var comandoArea = new SqlCommand(strQueryArea, minhaconexao);
            var retornoArea = comandoArea.ExecuteReader();
            #endregion

            #region Processa o retorno do select da raca
            //TODO: 3º Processar o retorno do banco de dados OU não

            var listaDeAreas = new List<Area>();

            while (retornoArea.Read())
            {
                var tempArea = new Area();
                tempArea.Id = int.Parse(retornoArea["idArea"].ToString());
                tempArea.Nome = retornoArea["Area"].ToString();
                listaDeAreas.Add(tempArea);
            }

            ViewBag.ListaDeAreas = listaDeAreas;
            #endregion
            return View(empresa);
        }

	}
}