using System;
using System.Collections.Generic;

// Interface IItem
public interface IItem
{
    void Apply(Character character);
}

// Interface IPerk
public interface IPerk
{
    void ApplyPerk(Character character);
}

// Character class with its parameters
public class Character
{
    public string Name { get; set; }
    public int MaxHitPoints { get; set; }
    public int CurrentHitPoints { get; private set; }
    public int BaseDamage { get; set; }
    public int BaseArmor { get; set; }
    
    // Inventory where the items are stored
    private List<IItem> _inventory;
    
    // List of the character's active perks
    private List<IPerk> _activePerks;

    // Class' constructor which initializes the parameters
    public Character(string name, int maxHitPoints, int baseDamage, int baseArmor)
    {
        Name = name;
        MaxHitPoints = maxHitPoints;
        CurrentHitPoints = maxHitPoints;
        BaseDamage = baseDamage;
        BaseArmor = baseArmor;
        _inventory = new List<IItem>();
        _activePerks = new List<IPerk>();
    }

    // Character's attack
    public int Attack()
    {
        return BaseDamage;
    }

    // Character's defense
    public int Defense()
    {
        return BaseArmor;
    }

    // Recover HP based on the selected amount
    public void Heal(int amount)
    {
        CurrentHitPoints += amount;
        if (CurrentHitPoints > MaxHitPoints)
        {
            CurrentHitPoints = MaxHitPoints;
        }
    }

    // Function for taking damage, thus lowering HP
    public void ReceiveDamage(int damage)
    {
        int damageTaken = Math.Max(0, damage - BaseArmor);
        CurrentHitPoints -= damageTaken;
        if (CurrentHitPoints < 0) CurrentHitPoints = 0;
        Console.WriteLine($"{Name} receives {damageTaken} damage. Current HP: {CurrentHitPoints}/{MaxHitPoints}");
    }

    // Add item to the inventory
    public void AddItem(IItem item)
    {
        _inventory.Add(item);
    }

    // Apply all the inventory items
    public void ApplyItems()
    {
        foreach (var item in _inventory)
        {
            item.Apply(this);
        }
    }

    // Add perk to the character
    public void AddPerk(IPerk perk)
    {
        _activePerks.Add(perk);
    }

    // Applying all active perks
    public void ApplyPerks()
    {
        foreach (var perk in _activePerks)
        {
            perk.ApplyPerk(this);
        }
    }
}

// Abstract class Weapon
public abstract class Weapon : IItem
{
    public string Name { get; set; }
    public int Damage { get; set; }

    public Weapon(string name, int damage)
    {
        Name = name;
        Damage = damage;
    }

    // Applies weapon to the character enhancing its damge
    public void Apply(Character character)
    {
        character.BaseDamage += Damage;
        Console.WriteLine($"{Name} applied. {character.Name}'s damage increased by {Damage}.");
    }
}

// Sword class, inherits from Weapon class
public class Sword : Weapon
{
    public Sword() : base("Sword", 10) { }
}

// Axe class, inherits from Weapon class
public class Axe : Weapon
{
    public Axe() : base("Axe", 15) { }
}

// Abstract class Protection
public abstract class Protection : IItem
{
    public string Name { get; set; }
    public int Armor { get; set; }

    public Protection(string name, int armor)
    {
        Name = name;
        Armor = armor;
    }

    // Applies Protection to the character enhancing its defense
    public void Apply(Character character)
    {
        character.BaseArmor += Armor;
        Console.WriteLine($"{Name} applied. {character.Name}'s armor increased by {Armor}.");
    }
}

// Shield class, inherits from Protection class
public class Shield : Protection
{
    public Shield() : base("Shield", 5) { }
}

// Helmet class, inherits from Protection class
public class Helmet : Protection
{
    public Helmet() : base("Helmet", 3) { }
}

// Perk which gives extra damage
public class ExtraDamagePerk : IPerk
{
    private int extraDamage;

    public ExtraDamagePerk(int damage)
    {
        extraDamage = damage;
    }

    public void ApplyPerk(Character character)
    {
        character.BaseDamage += extraDamage;
        Console.WriteLine($"{character.Name} gained {extraDamage} extra damage from perk.");
    }
}

// Perk which gives extra armor
public class ExtraArmorPerk : IPerk
{
    private int extraArmor;

    public ExtraArmorPerk(int armor)
    {
        extraArmor = armor;
    }

    public void ApplyPerk(Character character)
    {
        character.BaseArmor += extraArmor;
        Console.WriteLine($"{character.Name} gained {extraArmor} extra armor from perk.");
    }
}

// Perk which gives extra HP
public class ExtraHealthPerk : IPerk
{
    private int extraHealth;

    public ExtraHealthPerk(int health)
    {
        extraHealth = health;
    }

    public void ApplyPerk(Character character)
    {
        character.MaxHitPoints += extraHealth;
        Console.WriteLine($"{character.Name} gained {extraHealth} extra HP from perk.");
    }
}

// Perk which gives extra damage at the cost of some defense
public class OffensiveStancePerk : IPerk
{
    private int attackIncrease;
    private int defenseDecrease;

    public OffensiveStancePerk(int attackIncrease, int defenseDecrease)
    {
        this.attackIncrease = attackIncrease;
        this.defenseDecrease = defenseDecrease;
    }

    public void ApplyPerk(Character character)
    {
        character.BaseDamage += attackIncrease;
        character.BaseArmor = Math.Max(0, character.BaseArmor - defenseDecrease); 
        Console.WriteLine($"{character.Name} entered Offensive Stance: +{attackIncrease} attack, -{defenseDecrease} defense.");
    }
}

// Perk which gives extra defense at the cost of some attack
public class DefensiveStancePerk : IPerk
{
    private int defenseIncrease;
    private int attackDecrease;

    public DefensiveStancePerk(int defenseIncrease, int attackDecrease)
    {
        this.defenseIncrease = defenseIncrease;
        this.attackDecrease = attackDecrease;
    }

    public void ApplyPerk(Character character)
    {
        character.BaseArmor += defenseIncrease;
        character.BaseDamage = Math.Max(0, character.BaseDamage - attackDecrease); 
        Console.WriteLine($"{character.Name} entered Defensive Stance: +{defenseIncrease} defense, -{attackDecrease} attack.");
    }
}

// Program class, which contains the main function
public class Program
{
    public static void Main(string[] args)
    {
        // Creating the character
        Character hero = new Character("Hero", 100, 20, 5);

        // Creating items
        IItem sword = new Sword();
        IItem shield = new Shield();

        // Adding items to the inventory
        hero.AddItem(sword);
        hero.AddItem(shield);

        // Applying items in the inventory to the character
        hero.ApplyItems();

        // Adding perks to the character
        IPerk damagePerk = new ExtraDamagePerk(5);
        IPerk armorPerk = new ExtraArmorPerk(3);
        IPerk healthPerk = new ExtraHealthPerk(20);

        // Adding perks with secondary effects to the character
        IPerk offensiveStance = new OffensiveStancePerk(8, 4);
        IPerk defensiveStance = new DefensiveStancePerk(7, 3);

        hero.AddPerk(damagePerk);
        hero.AddPerk(armorPerk);
        hero.AddPerk(healthPerk);
        hero.AddPerk(offensiveStance); 
        hero.AddPerk(defensiveStance);  

        // Applying perks to the character
        hero.ApplyPerks();

        // Character's current attack and defense stats
        Console.WriteLine($"{hero.Name} attacks with {hero.Attack()} damage.");
        Console.WriteLine($"{hero.Name} defends with {hero.Defense()} armor.");

        // Receiving the selected damage amount
        hero.ReceiveDamage(30);

        // Healing the character
        hero.Heal(20);
        Console.WriteLine($"{hero.Name} healed. Current HP: {hero.CurrentHitPoints}/{hero.MaxHitPoints}");
    }
}
