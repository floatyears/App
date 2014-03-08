public class PartyFocusState{
	
	public PartyFocusState(){
		isFocus = false;
		isChanged = false;
		position = 0;
		data = null;
	}
	
	private bool isFocus;
	public bool IsFocus{
		get{
			return isFocus;
		}
		set{
			isFocus = value;
		}
	}
	
	private bool isChanged;
	public bool IsChanged{
		get{
			return isChanged;
		}
		set{
			isChanged = value;
		}
	}
	
	private int position;
	public int Position{
		get{
			return position;
		}
		set{
			position = value;
		}
	}
	
	private TUserUnit data;
	public TUserUnit Data{
		get{
			return data;
		}
		set{
			data = value;
		}
	}
	
	public void ResetState(){
		isFocus = false;
		isChanged = false;
		position = 0;
		data = null;
	}
}