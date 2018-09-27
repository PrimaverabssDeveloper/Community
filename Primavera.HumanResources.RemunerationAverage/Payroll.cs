using Primavera.Extensibility.BusinessEntities.ExtensibilityService.EventArgs;
using Primavera.Extensibility.HumanResources.Editors;
using RhpBE100;
using StdBE100;

namespace Primavera.Extensibility.HumanResources
{
    public class Payroll : ProcessamentoManual
    {

        public override void AntesDeGravar(ref bool Cancelar, ref bool NaoExecutaRecalculo, ExtensibilityEventArgs e)
        {
            try
            {
                ProcessAllowance(this.DadosProcessamento);
            }
            catch 
            {
                //An error occurred while processing your request
            }
        }

        private void ProcessAllowance(RhpBEProcessamento Processing)
        {
            const string ShiftAllowance = "R05";
            const string NightAllowance = "R20";
            const string AddHolidayAllowance = "R54";
            const string AddChristmasAllowance = "R55";
            
            RhpBERemuneracao remuneration = new RhpBERemuneracao();
            StdBELista average;

            if ((Processing.TipoProcessamento == TipoProc.tpSubsFerias || Processing.TipoProcessamento == TipoProc.tpSubsNatal) && (!string.IsNullOrEmpty(Processing.Funcionario)))
            {

                if (Processing.TipoProcessamento == TipoProc.tpSubsFerias)
                    remuneration = BSO.RecursosHumanos.Remuneracoes.Edita(AddHolidayAllowance);
                else
                    remuneration = BSO.RecursosHumanos.Remuneracoes.Edita(AddChristmasAllowance);

                if (remuneration != null)
                {
                    //Calculate the average remuneration
                    string sql = "SELECT ISNULL( ROUND(SUM(Valor)/(COUNT(DISTINCT NumProc)), 2), 0) Media ";
                    sql += "FROM MovimentosFuncionarios ";
                    sql += $"WHERE Funcionario ={DadosProcessamento.Funcionario}' AND Ano = {DadosProcessamento.Ano} AND MesFiscal < {DadosProcessamento.MesProcessamento} AND CodMov IN ('{ShiftAllowance}', '{NightAllowance}') AND TipoVenc = 1";

                    average = BSO.Consulta(sql);

                    double value = average.Valor("Media");

                    //Creates the remuneration in the processing, if the average value is different from zero, 
                    if (value > 0) 
                    {
                        RhpBEProcRemuneracao processingRemuneration = new RhpBEProcRemuneracao
                        {
                            IDLinhaOrigem = string.Empty,
                            IdFuncRemCBL = string.Empty,
                            IDLinhaRecalc = 0,
                            NumProcRecalc = 0,
                            Remuneracao = remuneration.Remuneracao,
                            Descricao = remuneration.Descricao,
                            TabelaIRSFixa = remuneration.TabelaIRSFixa,
                            TipoRemuneracao = tpRem.tpRemValorTotal,
                            TipoSubsidioAlimentacao = RhpBETipos.RHPTiposSubsidiosAlimentacao.Indefinido,
                            TipoCalculo = tpCalculo.tpCalcValorFixo,
                            Percentual = false,
                            Percentagem = 0,
                            ValorIntroducao = value,
                            Valor = value,
                            ValorIliquido = value,
                            ValorIliquidoEfectivo = value,
                            ValorUnitario = value,
                            Unidades = 1,
                            UnidadesLiquido = 1,
                            MovNaoRegular = true,
                            Origem = OrigemDados.origemSistema,
                            Sistema = true,
                            AlteradoVBA = true,
                            AnoReferencia = Processing.Ano,
                            PeriodoReferencia = Processing.MesProcessamento,
                            MesFiscalReferencia = Processing.MesProcessamento,
                            MesCalculoIRS = DadosProcessamento.MesProcessamento,
                            CalculoDiferidoAtivo = DadosProcessamento.CalculoDiferidoAtivo,
                            Instrumento = DadosProcessamento.Instrumento,
                            Situacao = DadosProcessamento.Situacao,
                            TipoVencimento = (int)Processing.TipoProcessamento
                        };

                        try
                        {
                            //Add remuneration to processing
                            Processing.Remuneracoes.Insere(processingRemuneration, remuneration.Remuneracao);
                        }
                        catch 
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}