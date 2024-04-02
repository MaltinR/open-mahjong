using OpenMahjong.Data;
using OpenMahjong.Equipment;
using OpenMahjong.Equipment.Set;
using OpenMahjong.Experiment;
using OpenMahjong.Games;
using OpenMahjong.Util;
using System.Collections.ObjectModel;

public class Program
{

    static void Main(string[] args)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Test02();
        watch.Stop();
        Console.WriteLine($"Running time {watch.ElapsedMilliseconds}");
    }

    static void Test02()
    {
        // SetBase set = new BasicSet();
        // var tiles = set.GetTiles();

        // 00000001
        Suit bambooSuit = new Suit("bam", "Bamboo", 1 << 0, true, true, true);
        // 00000010
        Suit dotSuit = new Suit("dot", "Dot", 1 << 1, true, true, true);
        // 00000100
        Suit charSuit = new Suit("cha", "Character", 1 << 2, true, true, true);
        // 00001000
        Suit honorSuit = new Suit("hon", "Honor", 1 << 3, false, true, true);

        List<Tile> tiles = new List<Tile>
        {
            /*
            new Tile(0, bambooSuit, "B1"),
            new Tile(1, bambooSuit, "B2"),
            new Tile(2, bambooSuit, "B3"),
            new Tile(0, dotSuit, "D1"),
            new Tile(0, dotSuit, "D1"),
            new Tile(0, dotSuit, "D1"),
            new Tile(1, dotSuit, "D2"),
            new Tile(2, dotSuit, "D3"),
            new Tile(3, dotSuit, "D4"),
            new Tile(4, dotSuit, "D5"),
            new Tile(2, honorSuit, "H3"),
            new Tile(2, honorSuit, "H3"),
            new Tile(2, honorSuit, "H3"),
            */

            // 九子連環
            /*
            new Tile(0, bambooSuit, "B1"),
            new Tile(0, bambooSuit, "B1"),
            new Tile(0, bambooSuit, "B1"),
            new Tile(1, bambooSuit, "B2"),
            new Tile(2, bambooSuit, "B3"),
            new Tile(3, bambooSuit, "B4"),
            new Tile(4, bambooSuit, "B5"),
            new Tile(5, bambooSuit, "B6"),
            new Tile(6, bambooSuit, "B7"),
            new Tile(7, bambooSuit, "B8"),
            new Tile(8, bambooSuit, "B9"),
            new Tile(8, bambooSuit, "B9"),
            new Tile(8, bambooSuit, "B9"),
            */

            // 七對子
            new Tile(0, bambooSuit, "B1"),
            new Tile(0, bambooSuit, "B1"),
            new Tile(2, bambooSuit, "B3"),
            new Tile(2, bambooSuit, "B3"),
            new Tile(0, dotSuit, "D1"),
            new Tile(0, dotSuit, "D1"),
            new Tile(2, dotSuit, "D3"),
            new Tile(2, dotSuit, "D3"),
            new Tile(3, dotSuit, "D4"),
            new Tile(3, dotSuit, "D4"),
            new Tile(1, honorSuit, "H2"),
            new Tile(2, honorSuit, "H3"),
            new Tile(2, honorSuit, "H3"),
        };

        TilesStat tilesStat = new TilesStat(tiles);
        List<Suit> suits = new List<Suit> { bambooSuit, dotSuit, charSuit, honorSuit, };
        ReadOnlyCollection<Suit> readOnlySuits = suits.AsReadOnly();

        var winningTilesNormal = new WinningPatternNormal().
            GetWinningTiles(tilesStat, readOnlySuits);
        var winningTilesAllPairs = new WinningPatternAllPairs().
            GetWinningTiles(tilesStat, readOnlySuits);

        winningTilesNormal.Sort();
        winningTilesAllPairs.Sort();

        Console.WriteLine($"Normal: {string.Join(", ", winningTilesNormal)}");
        Console.WriteLine($"All pairs: {string.Join(", ", winningTilesAllPairs)}");
    }

    static void Test01()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        SetBase set = new BasicSet();

        Player[] players = new Player[]
        {
        new Player("testPlayer01", "Player 01"),
        new Player("testPlayer02", "Player 02"),
        new Player("testPlayer03", "Player 03"),
        new Player("testPlayer04", "Player 04"),
        };

        GameManager gameManager = new GameManager(players, 3, set);
        gameManager.GamesFinished += OnGamesFinished;

        // var tiles = set.GetTiles();

        for (int i = 0; i < gameManager.Players.Length; i++)
        {
            gameManager.Players[i].TileDrawn += OnTileDrawn;
        }

        int counter = 0;

        do
        {
            gameManager.Start();
            counter++;
        }
        while (gameManager.Next());

        void OnTileDrawn(InGamePlayer player, Tile tile)
        {
            // Console.WriteLine($"OnTileDrawn");
            gameManager.Discard(tile, player);
        }

        void OnGamesFinished(GameManager gameManager)
        {
            watch.Stop();
            Console.WriteLine($"Init time {watch.ElapsedMilliseconds}");
        }
    }
}