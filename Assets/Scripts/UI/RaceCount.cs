//Class to create a hashmap<Race, int>
public class RaceCount
{
    private Race r;
    private int number;
    private string def;

    public RaceCount(Race race, int n)
    {
        r = race;
        number = n;
        initDefinition(0);
    }

    public int getNumber()
    {
        return number;
    }

    public Race getRace()
    {
        return r;
    }

    public string getString()
    {
        return def;
    }

    public void setNumber(int n)
    {
        number = n;
    }

    public void initDefinition(int lvl)
    {
        switch (r)
        {
            case Race.Orc:
                def += "Brutalité extrême\n<b>Sort</b>\nLes attaques des orcs ignorent 30% Orc la défense de l'ennemi mais perdent de la précision\n";
                if (lvl == 0)
                    def += "(2) 5 secondes, -10% de précision";
                else if (lvl == 1)
                    def += "<b>(2) 5 secondes, -10% de précision</b>";
                break;

            case Race.Skeleton:
                def += "Monde des morts\n<b>Sort</b>\nLes ennemis perdent de l'armure pendant 5 secondes\n";
                if (lvl == 0)
                    def += "(2) -25% d'armure";
                else if (lvl == 1)
                    def += "<b>(2) -25% d'armure</b>";
                break;

            case Race.Octopus:
                def = "Cage tentaculaire\n<b>Sort</b>\nEtourdi la cible\n";
                if (lvl == 0)
                    def += "(2) 5 secondes";
                else if (lvl == 1)
                    def += "<b>(2) 5 secondes</b>";
                break;

            case Race.Elemental:
                def = "Fusion des éléments\n<b>Sort</b>\nL'ennemi ciblé subit des dégâts pour chaque élémentaire en jeu\n";
                if (lvl == 0)
                    def += "(2) 10 dégâts par élémentaire";
                else if (lvl == 1)
                    def += "<b>(2) 10 dégâts par élémentaire</b>";
                break;

            case Race.Giant:
                def = "Impact titanesque\n<b>Sort</b>\nSélectionnez un géant, sa prochaine attaque est plus puissante et étourdi la cible pendant 2 secondes\n";
                if (lvl == 0)
                    def += "(2) + 15% de dégâts";
                else if (lvl == 1)
                    def += "<b>(2) + 15% de dégâts</b>";
                break;

            case Race.Ratman:
                def = "Morsure empoisonnées\n<b>Sort</b>\nLa prochaine attaque de tous les hommes-rats empoisonne leur cible pendant 5 secondes\n";
                if (lvl == 0)
                    def += "(2) 2 dégâts /seconde";
                else if (lvl == 1)
                    def += "<b>(2) 2 dégâts /seconde</b>";
                break;

            case Race.Demon:
                def = "Je suis enfin complet\n<b>Sort</b>\nInvoque le roi démon sur la cas ciblée, il possède moins de vie mais inflige plus de dégâts\n";
                if (lvl == 0)
                    def += "(3) -50% de vie, + 30% de dégâts";
                if (lvl == 1)
                    def += "<b>(3) -50% de vie, + 30% de dégâts</b>";
                break;
        }

    }
}
