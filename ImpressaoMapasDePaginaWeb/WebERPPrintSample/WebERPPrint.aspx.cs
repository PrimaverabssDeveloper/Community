using System;
using System.Diagnostics;
using System.Text;

namespace WebERPPrintSample
{
    /// <summary>
    /// New webform
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class WebForm1 : System.Web.UI.Page
    {
        dynamic plat_;
        dynamic conf_;

        string version;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            version = ddlVersao.SelectedItem.Text;
        }

        /// <summary>
        /// Handles the Click event of the Button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            this.lblOutput.Text = string.Empty;
            Global.EstadoImpressao = string.Empty;
            ImprimeListagem();
        }
        
        private void ImprimeListagem()
        {
            lblOutput.Text = string.Empty;

            StringBuilder strF = new StringBuilder(); // Formula
            StringBuilder strP = new StringBuilder(); // Parametros
            StringBuilder strS = new StringBuilder(); // SelFormula

            try
            {
                Global.EstadoImpressao = $"A preparar impressão...";
                PrintUpdate.Update();

                int copias = 1;
                if (int.TryParse(txtCopies.Text, out copias))
                {
                    if (copias > 10)
                    {
                        copias = 10;
                        Global.EstadoImpressao = "Número de cópias foi reduzido para 10!";
                    }
                    else if (copias <= 0)
                    {
                        copias = 1;
                        Global.EstadoImpressao = "Número de cópias foi alterado para 1!";
                    }
                }
                else
                {
                    throw new Exception("Número de cópias inválido!");
                }

                string sComp = txtEmpresa.Text;

                // para ter o intellisense, terão que ser adicionadas referências ao projecto (interops).

                plat_ = Activator.CreateInstance(Type.GetTypeFromProgID("StdPlatBS" + version + ".StdPlatBS", true));
                conf_ = Activator.CreateInstance(Type.GetTypeFromProgID("StdPlatBS" + version + ".StdBSConfApl", true));

                conf_.AbvtApl = "GCP";
                conf_.Instancia = "default";
                conf_.Utilizador = txtUtil.Text;
                conf_.PwdUtilizador = txtPassword.Text;
                conf_.LicVersaoMinima = ddlVersao.SelectedItem.Value;

                dynamic oTrans = null;

                // Abrir empresa de trabalho
                // Para utilizar este método é necessário ter licença de motores
                if (version == "900") plat_.AbrePlataformaEmpresa(sComp, oTrans, conf_, 0, string.Empty);
                else plat_.AbrePlataformaEmpresaIntegrador(sComp, oTrans, conf_, 0);

                for (int i = 0; i < copias; i++)
                {
                    Global.EstadoImpressao = $"Impressão da cópia {i + 1}...";

                    // Tipo Documento/Série e Nº
                    strS = new StringBuilder("{CabecDoc.TipoDoc}='" + txtDoc.Text + "' and {CabecDoc.Serie} = '" + txtSerie.Text + "' AND {CabecDoc.NumDoc}=" + txtNum.Text);

                    // Inicialização do módulo ao qual corresponde o mapa
                    plat_.Mapas.Inicializar("GCP");

                    strF.Append("StringVar Nome:='" + plat_.Contexto.Empresa.IDNome + "';");
                    strF.Append("StringVar Morada:='" + plat_.Contexto.Empresa.IDMorada + "';");
                    strF.Append("StringVar Localidade:='" + plat_.Contexto.Empresa.IDLocalidade + "';");
                    strF.Append("StringVar CodPostal:='" + plat_.Contexto.Empresa.IDCodPostal + " " + plat_.Contexto.Empresa.IDCodPostalLocal + "';");
                    strF.Append("StringVar Telefone:='" + plat_.Contexto.Empresa.IDTelefone + "';");
                    strF.Append("StringVar Fax:='" + plat_.Contexto.Empresa.IDFax + "';");
                    strF.Append("StringVar Contribuinte:='" + plat_.Contexto.Empresa.IFNIF + "';");
                    strF.Append("StringVar CapitalSocial:='" + plat_.Contexto.Empresa.ICCapitalSocial + "';");
                    strF.Append("StringVar Conservatoria:='" + plat_.Contexto.Empresa.ICConservatoria + "';");
                    strF.Append("StringVar Matricula:='" + plat_.Contexto.Empresa.ICMatricula + "';");
                    strF.Append("StringVar MoedaCapitalSocial:='" + plat_.Contexto.Empresa.ICMoedaCapSocial + "';");

                    plat_.Mapas.SetFormula("DadosEmpresa", strF.ToString());

                    strP.Append("NumberVar TipoDesc;");
                    strP.Append("NumberVar DecQde;");
                    strP.Append("NumberVar DecPrecUnit;");
                    strP.Append("BooleanVar UltimaPag;");
                    strP.Append("TipoDesc:= 0;");
                    strP.Append("DecQde:=3;");
                    strP.Append("UltimaPag:=False;");

                    plat_.Mapas.SetFormula("InicializaParametros", strP.ToString());

                    if (ckbImp.Checked)
                    {
                        plat_.Mapas.DefinicaoImpressoraIntegracao("CutePDF Writer", "CutePDF Writer", "CutePDF Writer", "CPW2:",
                                                                   1, 600, 9, 27, 21, 1, 0, 1, 1, 1);
                    }

                    plat_.PrefUtilStd.UsaImpressoraMapas = 0;
                    plat_.Mapas.Destino = 0;
                    plat_.Mapas.SetFileProp(8, $"C:\\temp\\outputPrint{i + 1}.pdf");
                    plat_.Mapas.ImprimeListagem(txtReport.Text, "Factura", "P", 3, "N", strS.ToString(), 0, false, true);
                }

                plat_.FechaPlataformaEx();
            }
            catch (Exception e)
            {
                lblOutput.Text = e.ToString();
            }
            finally
            {
                Global.EstadoImpressao = "Impressão com sucesso!";

                plat_ = null;
                conf_ = null;
            }
        }
        
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVersao.SelectedItem.Text == "800")
            {
                Session.Add("Version", 800);
            }
            else
            {
                Session.Add("Version", 900);
            }

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.lblOutput.Text = string.IsNullOrEmpty(Global.EstadoImpressao) ? "Em espera..." : Global.EstadoImpressao;
            PrintUpdate.Update();
            Debug.Print("Updated!!");
        }
    }
}