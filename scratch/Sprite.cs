namespace Scratch;

public partial class Sprite
{
    public string Name = "Sprite";
    
    public List<Costume> Costumes = [];
    public int CurrentCostume { get; private set; } = 0;
    
    public float X = 0;
    public float Y = 0;
    
    public double Size = 100;

    public double Direction = 90;
    public string RotationStyle = "";

    public bool Visible = true;
    public int LayerOrder = 0;
    
    public Blocks Blocks = new Blocks();
    
    public Sprite(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }

    public virtual void AddCostume(Costume costume, int index = 0)
    {
        Costumes.Insert(index, costume);
    }

    public virtual void SetCostume(string CostumeName)
    {
        int newCostume = -1;

        for (int i = 0; i < Costumes.Count; i++) {
            Costume costume = Costumes[i];
            if (costume.Name ==  CostumeName)
            {
                newCostume = i;
                break;
            }
        }

        if (newCostume > -1)
            CurrentCostume = newCostume;
    }

    public virtual void SetCostume(int index)
    {
        CurrentCostume = index;
    }

    public virtual void ReadBlocks()
    {

    }

    public override string ToString()
    {
        return $"Sprite(Name: {Name}, X: {X}, Y: {Y}, Size: {Size}, Visible: {Visible})";
    }
}
