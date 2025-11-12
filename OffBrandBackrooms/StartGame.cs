using OffBrandBackrooms;

namespace OffBrandBackrooms
{
  public class StartGame : Command
  {
    public StartGame() : base("SG") { }

    override
    public Boolean Execute(CommandFunctions commandfunctions)
    {
        Boolean result = false;
        result = commandfunctions.NewGame();
        return result;
    }

    override
    public Boolean Undo(CommandFunctions commandfunctions)
    {
        Boolean result = false;

        result = true;
        return result;
    }
  }
}