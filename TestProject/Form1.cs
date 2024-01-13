using NovelTextProcessor;
using NovelTextProcessor.Dtos;

namespace NewNovelTranslator;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    ~Form1()
    {
        ThreadSafeHttpClientSingleton.Instance.Dispose();
    }

    private async void start_Click(object sender, EventArgs e)
    {
        //DocumentProcessor docprocessor = new DocumentProcessor(text.Text);

        ////var result = docprocessor.ExtractEntityNames();

        //foreach (var span in docprocessor.document) 
        //{
        //    var test = span.TokenizedValue;
        //}


        var seedData = new List<EntityName>
        {
            new() { EnglishName = "Angus", ArabicName = "�����", Gender = 'M' },
            new() { EnglishName = "Jayna", ArabicName = "����", Gender = 'F' },
            new() { EnglishName = "Jade", ArabicName = "����", Gender = 'M' }
        };

        var processor = new Processor(text.Text, seedData.ToArray());
        await processor.RunAsync();

        textResult.Text = processor.GetResult();

        //MessageBox.Show(processor.GetResult());

        //NamedEntityRecognition.OnnxModelRunner.Test();
        //Console.WriteLine(result);
    }
}