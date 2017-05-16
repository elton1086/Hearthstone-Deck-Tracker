using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Hearthstone_Deck_Tracker.Annotations;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility;

namespace Hearthstone_Deck_Tracker.FlyoutControls.DeckExport
{
	public class DeckExportViewModel : INotifyPropertyChanged
	{
		private Deck _deck;
		private string _deckString;
		private string _copyAllButtonText = "Copy all";
		private string _copyCodeButtonText = "Copy code only";

		public Deck Deck
		{
			get { return _deck; }
			set
			{
				_deck = value;
				DeckString = DeckSerializer.Serialize(value);
				OnPropertyChanged();
			}
		}

		public string DeckString
		{
			get { return BuildFullDeckString(_deckString); }
			set
			{
				_deckString = value;
				OnPropertyChanged();
			}
		}

		public string CopyAllButtonText
		{
			get { return _copyAllButtonText; }
			set
			{
				_copyAllButtonText = value;
				OnPropertyChanged();
			}
		}

		public string CopyCodeButtonText
		{
			get { return _copyCodeButtonText; }
			set
			{
				_copyCodeButtonText = value;
				OnPropertyChanged();
			}
		}

		public ICommand CopyAllCommand => new Command(CopyAll);

		public ICommand CopyCodeCommand => new Command(CopyCode);

		public async void CopyAll()
		{
			if(Deck == null)
				return;
			Clipboard.SetText(DeckString);
			CopyAllButtonText = "COPIED!";
			await Task.Delay(2000);
			CopyAllButtonText = "COPY ALL";
		}

		public async void CopyCode()
		{
			if(Deck == null)
				return;
			Clipboard.SetText(_deckString);
			CopyCodeButtonText = "COPIED!";
			await Task.Delay(2000);
			CopyCodeButtonText = "COPY CODE ONLY";
		}

		public string BuildFullDeckString(string deckString)
		{
			if(_deck == null)
				return deckString;

			var sb = new StringBuilder();
			sb.AppendLine($"### {_deck.Name}");
			sb.AppendLine($"# Class: {_deck.Class}");
			sb.AppendLine($"# Format: {(_deck.IsWildDeck ? "Wild" : "Standard")}");
			sb.AppendLine("#");
			foreach(var card in _deck.Cards)
				sb.AppendLine($"# {card.Count}x ({card.Cost}) {card.Name}");
			sb.AppendLine("#");
			sb.AppendLine(deckString);
			sb.AppendLine("#");
			sb.AppendLine("# To use this deck, copy it to your clipboard and crate a new deck in Hearthstone");
			return sb.ToString();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}