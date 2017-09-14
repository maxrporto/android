using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.Font.FontFamily;
using System.IO;
using System.Reflection;
using System.Data;

namespace ClassLibrary1
{
    class Eventos : PdfPageEventHelper
    {
        // propriedade da fonte que será usada no cabeçalho
        public Font fonte { get; set; }

        // a classe recebe a fonte no seu construtor a classe não possui construtor padrão, para obrigar
        // a passagem da fonte e evitar erros
        public Eventos(Font fonte_)
        {
            fonte = fonte_;
        }

        // Este método cria um cabeçalho para o documento
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            // Cria um novo paragrafo com o texto do cabeçalho
            Paragraph ph = new Paragraph("Teste cabeçalho Pdf ", fonte);

            // adiciono a linha e posteriormente mais linhas que podem ser necessárias em um cabeçalho de relatório
            document.Add(ph);
            ph = new Paragraph("Teste Pdf 1 ", fonte);
            document.Add(ph);
            ph = new Paragraph("Teste Pdf 2 ", fonte);
            document.Add(ph);
            ph = new Paragraph("Teste Pdf 3 ", fonte);
            document.Add(ph);

            // cria um novo paragrafo para imprimir um traço e uma linha em branco
            ph = new Paragraph();

            // cria um objeto sepatador (um traço)
            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();

            // adiciona o separador ao paragravo
            ph.Add(seperator);

            // adiciona a linha em branco(enter) ao paragrafo
            ph.Add(new Chunk("\n"));

            // imprime o pagagrafo no documento
            document.Add(ph);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // para o rodapé é um pouco diferente precisamos criar um PdfContentByte e uma BaseFont e
            // setar as propriedades dos mesmos para então poder imprimir alinhado a direita

            // cria uma instancia da classe PdfContentByte
            PdfContentByte cb = writer.DirectContent;

            // cria uma instancia da classe font
            BaseFont font;

            // seta as propriedades da fonte
            font = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

            // seta a fonte do objeto PdfContentByte
            cb.SetFontAndSize(font, 9);

            // escreve a linha para imprimir o numero da página
            string texto = "Página: " + writer.PageNumber.ToString();

            // imprime a linha no rodapé
            cb.ShowTextAligned(Element.ALIGN_RIGHT, texto, document.Right, document.Bottom - 20, 0);
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Document document = new Document();
            document.SetPageSize(PageSize.A4);
            try
            {
                // cria o arquivo pdf
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("teste.pdf", FileMode.Create));

                // cria um objeto do tipo FontFamily, que contem as propriedades de uma fonte
                Font.FontFamily familha = new Font.FontFamily();

                // atribui a familia da fonte, no caso Courier
                familha = iTextSharp.text.Font.FontFamily.COURIER;

                // cria uma fonte atribuindo a familha, o tamanho da fonte e o estilo (normal, negrito...)
                Font fonte = new Font(familha, 8, (int)System.Drawing.FontStyle.Bold);

                // cria uma instancia da classe eventos, é uma classe que mostrarei posteriormente
                // esta clase trata a criação do cabeçalho e rodapé da página
                Eventos ev = new Eventos(fonte);

                // seta o atributo de eventos da classe com a variavel de eventos criada antes
                writer.PageEvent = ev;

                // altera a fonte para normal, a negrito era apenas para o cabeçalho e rodapé da página
                fonte = new Font(familha, 8, (int)System.Drawing.FontStyle.Regular);

                // abre o documento para começar a escrever o pdf
                document.Open();

                // aqui faz um for para simular diversas linhas de um relatorio
                for (int i = 0; i < 100; i++)
                {
                    // adiciona um novo paragrafo com o texto da respectiva linha.
                    document.Add(new Paragraph("Teste linha", fonte));
                }
            }
            catch (Exception de)
            {
                MessageBox.Show(de.Message);
            }

            // fecha o documento
            document.Close();

            // manda abrir o pdf
            Process.Start("teste.pdf");
        }
    }
    //public class Class1
    //{
    //    public static bool RelRespDespDia(string Data, string caminhoPDF, int Empresa)
    //    {

    //        DataTable dt = new DataTable();
    //        bool _retorno;
    //        dt = clsConsulta.RetornaResumoDespDia(Convert.ToDateTime(Data), Empresa);
    //        if (dt != null)
    //        {
    //            // defino documento
    //            Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
    //            try
    //            {
    //                // cria o arquivo
    //                PdfWriter.GetInstance(document, new FileStream(caminhoPDF, FileMode.Create));
    //                // we add some meta information to the document
    //                document.AddAuthor("AFK");
    //                document.AddSubject(".");
    //                document.Open();

    //                iTextSharp.text.Table TableCabec = new iTextSharp.text.Table(1);
    //                TableCabec.WidthPercentage = 100;
    //                TableCabec.Padding = 2;
    //                //alinhamento dentro da celula
    //                TableCabec.Spacing = 0;
    //                TableCabec.DefaultCellBorderWidth = 0;
    //                TableCabec.DefaultHorizontalAlignment = 1;
    //                TableCabec.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //                TableCabec.Alignment = Element.ALIGN_CENTER;
    //                TableCabec.BackgroundColor = new iTextSharp.text.Color(0xC0, 0xC0, 0xC0);
    //                TableCabec.AddCell(new Phrase(@"Resumo Despesas Diária", FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
    //                TableCabec.AddCell(new Phrase("Data Base: " + Data + " Data Emissão:  " + DateTime.Now, FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD)));
    //                TableCabec.EndHeaders();
    //                document.Add(TableCabec);

    //                //rodapé
    //                HeaderFooter header = new HeaderFooter(new Phrase("Data Base: " + Data + " Data Emissão:  " + DateTime.Now + "  Pagina : "), true);
    //                document.Footer = header;

    //                //Cabeçalho empresa
    //                iTextSharp.text.Table TableFor = new iTextSharp.text.Table(1);
    //                float[] headerwidthsFor = { 100 };
    //                TableFor.Widths = headerwidthsFor;
    //                TableFor.AutoFillEmptyCells = true;
    //                TableFor.WidthPercentage = 100;
    //                TableFor.Spacing = 0;
    //                TableFor.DefaultCellBorderWidth = 1;
    //                TableFor.DefaultHorizontalAlignment = 1;
    //                TableFor.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //                //TableFor.Alignment = Element.ALIGN_CENTER;
    //                //TableFor.AddCell(new Phrase(@"Cod. For.", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 0));
    //                TableFor.Alignment = Element.ALIGN_LEFT;
    //                TableFor.AddCell(new Phrase(@"Empresa", FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD)));

    //                //Cabeçalho Itens.
    //                iTextSharp.text.Table TableItens = new iTextSharp.text.Table(8, 1);
    //                TableItens.AutoFillEmptyCells = true;
    //                float[] headerwidths = { 6, 26, 4, 4, 8, 8, 10, 34 };
    //                TableItens.Widths = headerwidths;
    //                TableItens.WidthPercentage = 100;
    //                TableItens.Spacing = 1;
    //                TableItens.DefaultCellBorderWidth = 1;
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                TableItens.AddCell(new Phrase(@"Cod. For.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 0));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                TableItens.AddCell(new Phrase(@"Razão", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 1));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                TableItens.AddCell(new Phrase(@"Doc.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 2));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                //TableItens.AddCell(new Phrase(@"Par.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 3));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                TableItens.AddCell(new Phrase(@"Tp Doc.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 3));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                TableItens.AddCell(new Phrase(@"Dt Emissão", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 4));
    //                //TableItens.AddCell(new Phrase(@"Dt Venc.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 6));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                TableItens.AddCell(new Phrase(@"Vl Docum.", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 5));
    //                //TableItens.AddCell(new Phrase(@"Gp.Despesas", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 6));
    //                //TableItens.AddCell(new Phrase(@"Tp.Despesa", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 7));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                TableItens.AddCell(new Phrase(@"Usuario", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 6));
    //                TableItens.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                TableItens.AddCell(new Phrase(@"Observação", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)), new System.Drawing.Point(0, 7));

    //                //TableFor.InsertTable(TableItens, new System.Drawing.Point(1, 1));
    //                //adiciona no documento
    //                TableFor.EndHeaders();
    //                TableItens.EndHeaders();
    //                document.Add(TableFor);
    //                document.Add(TableItens);
    //                //limpa variavel que ira controlar a quebra
    //                _nomeAnterior = string.Empty;

    //                foreach (DataRow row in dt.Rows)
    //                {
    //                    //trata a quebra por empresa criando uma nova tabela
    //                    if (_nomeAnterior != row["Nome_emp"].ToString())
    //                    {
    //                        iTextSharp.text.Table TableForr = new iTextSharp.text.Table(1);
    //                        float[] headerwidthsForr = { 100 };
    //                        TableForr.Widths = headerwidthsFor;
    //                        TableForr.AutoFillEmptyCells = true;
    //                        TableForr.WidthPercentage = 100;
    //                        TableForr.Spacing = 1;
    //                        TableForr.DefaultCellBorderWidth = 1;
    //                        TableForr.DefaultHorizontalAlignment = 1;
    //                        TableForr.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //                        TableForr.Alignment = Element.ALIGN_CENTER;
    //                        _nomeAnterior = row["Nome_emp"].ToString();
    //                        TableForr.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                        TableForr.AddCell(new Phrase(row["ID_Emp"].ToString() + " - " + row["Nome_emp"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                        //TableForr.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                        //TableForr.AddCell(new Phrase(row["Nome_emp"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                        document.Add(TableForr);
    //                    }
    //                    //insere os itens abaixo da quebra
    //                    iTextSharp.text.Table TableItenss = new iTextSharp.text.Table(8, 1);
    //                    TableItens.AutoFillEmptyCells = true;
    //                    float[] headerwidthss = { 6, 26, 4, 4, 8, 8, 10, 34 };
    //                    TableItenss.Widths = headerwidthss;
    //                    TableItenss.WidthPercentage = 100;
    //                    TableItenss.Spacing = 1;
    //                    TableItenss.DefaultCellBorderWidth = 1;
    //                    TableItenss.DefaultHorizontalAlignment = 1;
    //                    TableItenss.Border = iTextSharp.text.Rectangle.NO_BORDER;
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                    TableItenss.AddCell(new Phrase(row["ID_FOR"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                    TableItenss.AddCell(new Phrase(row["NM_RAZAO_SOCIAL"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    TableItenss.AddCell(new Phrase(row["ID_NFE_NF_ENT_FOR"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    //TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    //TableItenss.AddCell(new Phrase(row["SEQ_PARCELA"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    TableItenss.AddCell(new Phrase(row["CD_DOCUMENTO_PAGTO_ORIGINAL"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    TableItenss.AddCell(new Phrase(Convert.ToDateTime(row["DT_EMISSAO"]).ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    //TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    //TableItenss.AddCell(new Phrase(Convert.ToDateTime(row["DT_VENCIMENTO"]).ToString("dd/MM/yyyy"), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    TableItenss.AddCell(new Phrase(row["VL_DOCUMENTO"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    //TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    //TableItenss.AddCell(new Phrase(row["DS_GRUPO_DESPESA"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    //TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    //TableItenss.AddCell(new Phrase(row["DS_TIPO_DESPESA"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_CENTER;
    //                    TableItenss.AddCell(new Phrase(row["NM_USUARIO"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)));
    //                    TableItenss.DefaultHorizontalAlignment = Element.ALIGN_LEFT;
    //                    TableItenss.AddCell(new Phrase(row["DS_DOCUMENTO"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL)));

    //                    document.Add(TableItenss);
    //                }

    //                document.Close();
    //                _retorno = true;

    //            }
    //            catch (IOException ex)
    //            {
    //                _retorno = false;
    //            }
    //            catch (Exception ex)
    //            {
    //                _retorno = false;
    //            }
    //            finally
    //            {
    //                document.Close();
    //            }
    //            return _retorno;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}
}
