using NovelTextProcessor;

namespace NewNovelTranslator
{
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

		private void start_Click(object sender, EventArgs e)
		{
			//DocumentProcessor docprocessor = new DocumentProcessor(text.Text);

			////var result = docprocessor.ExtractEntityNames();

			//foreach (var span in docprocessor.document) 
			//{
			//    var test = span.TokenizedValue;
			//}


			//var seedData = new List<EntityName>
			//{
			//	new EntityName { EnglishName = "Angus", ArabicName = "�����", Gender = 'M' },
			//	new EntityName { EnglishName = "Jayna", ArabicName = "����", Gender = 'F' },
			//	new EntityName { EnglishName = "Jade", ArabicName = "����", Gender = 'M' },
			//};

			//Processor processor = new Processor(text.Text, seedData.ToArray());
			//await processor.RunAsync();

			//MessageBox.Show(processor.GetResult());

			//NamedEntityRecognition.OnnxModelRunner.Test();
			//Console.WriteLine(result);
		}
	}
}