
namespace Cards
{
	public enum CardUnitType : byte
	{
		None = 0,
		Murloc = 1,
		Beast = 2,
		Elemental = 3,
		Mech = 4
	}

	public enum SideType : byte
	{
		Common = 0,
		Mage = 1,
		Warrior = 2,
		Priest = 3,
		Warlock = 4,
		Hunter = 5
	}

	public enum CardStateType : byte
	{
		InDeck,
		InHand,
		OnTable
	}

	public enum FieldType : byte
	{
		HandOne,
		HandTwo,
		TableOne,
		TableTwo
	}
	
	public enum PlayerTypeCard : byte
	{
		OnePlayerCard,
		TwoPlayerCard
	}
}
