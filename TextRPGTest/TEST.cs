namespace TextRPGTest
{
    internal class TEST
    {
        static int level = 1;
        static string[] names = { "Chad" }; // 배열로 관리해보기
        static string[] jobs = { "전사" };
        static int attack = 10;
        static int defense = 5;
        static int hp = 100;
        static int money = 1500;

        public enum ItemType // 아이템 타입
        {
            Weapon,
            Armor,
            Ect
        }

        public class Item // 아이템 클래스
        {
            public string itemName; // 아이템 이름
            public int itemPrice;   // 아이템 가격
            public int itemAmount = 1; // 아이템 수량 1개로 지정
            public int itemStat;  // 아이템별 스탯
            public ItemType itemType; // 아이템 타입
            public string itemText;  // 아이템 각주

            public static List<Item> storeItems = new List<Item>();   // 판매하는 아이템 리스트
            public static List<Item> myItems = new List<Item>(); // 내가 산 아이템 리스트

            public Item(string itemName, int itemPrice, int itemAmount, int itemStat, ItemType itemType, string itemText) // 아이템 생성자
            {
                this.itemName = itemName;
                this.itemPrice = itemPrice;
                this.itemAmount = itemAmount;
                this.itemStat = itemStat;
                this.itemType = itemType;
                this.itemText = itemText;
            }

            public static void AddStoreItems() // 상점에 아이템 추가
            {
                storeItems.Add(new Item("검", 500, 1, 10, ItemType.Weapon, "강력한 검"));
                storeItems.Add(new Item("방패", 300, 1, 5, ItemType.Armor, "튼튼한 방패"));
                storeItems.Add(new Item("회복 물약", 100, 1, 0, ItemType.Ect, "체력을 회복하는 물약"));
            }

            public Item BuyItem(int index) // 아이템 구입
            {
                Item item = storeItems[index];

                if (money >= item.itemPrice)  // 돈이 충분할 때
                {
                    money -= item.itemPrice;
                    myItems.Add(item); // 구매한 아이템 리스트에 담는다
                    AddStat(item); // 플레이어의 스탯에 무기의 능력치를 추가한다
                    Console.WriteLine($"{item.itemName}을(를) 구매했습니다.");
                    return item;
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    return null;
                }
            }

            public void AddStat(Item item) // 능력치 추가
            {
                switch (item.itemType)
                {
                    case ItemType.Weapon: // 무기면 공격력 추가
                        attack += item.itemStat;
                        break;
                    case ItemType.Armor: // 방어구면 방어력 추가
                        defense += item.itemStat;
                        break;
                }
            }

            public static void ShowStoreItems() // 상점 아이템 출력
            {
                if (storeItems.Count == 0)
                {
                    Console.WriteLine("상점에 아이템이 없습니다.");
                }
                else
                {
                    Console.WriteLine("[상점 아이템 목록]");
                    for (int i = 0; i < storeItems.Count; i++)
                    {
                        var item = storeItems[i];
                        Console.WriteLine($"{i + 1}. {item.itemName} \t | 가격: {item.itemPrice} G \t | {item.itemText}");
                    }
                }
            }

            public static void ShowMyItems() // 내가 산 아이템 출력
            {
                if (myItems.Count == 0)
                {
                    Console.WriteLine("구매한 아이템이 없습니다.");
                }
                else
                {
                    Console.WriteLine("[내 아이템 목록]");
                    foreach (var item in myItems)
                    {
                        Console.WriteLine($"{item.itemName} \t | {item.itemText}");
                    }
                }
            }
        }

        static void StartScene() // 시작 페이지
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요: ");
            HandleInput(StartScene);
        }

        static void HandleInput(Action fallbackAction)
        {
            string Input = Console.ReadLine();
            switch (Input)
            {
                case "1": // 상태보기
                    Console.Clear();
                    StatusScene();
                    break;
                case "2": // 인벤토리
                    Console.Clear();
                    Inventory();
                    break;
                case "3": // 상점
                    Console.Clear();
                    Store();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    fallbackAction.Invoke();
                    break;
            }
        }

        static void StatusScene() // 상태보기 페이지
        {
            Console.WriteLine("[상태보기]");
            Console.WriteLine($"Lv. {level}");
            Console.WriteLine($"{names[0]} ({jobs[0]})");
            Console.WriteLine($"공격력: {attack}");
            Console.WriteLine($"방어력: {defense}");
            Console.WriteLine($"체력: {hp}");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            HandleInput(StatusScene);
        }

        static void Inventory() // 인벤토리 페이지
        {
            Console.WriteLine("[인벤토리]");
            Item.ShowMyItems();
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            HandleInput(Inventory);
        }

        static void Store() // 상점 페이지
        {
            Console.WriteLine("[상점]");
            Console.WriteLine($"보유 골드: {money} G");
            Item.ShowStoreItems();
            Console.WriteLine();
            Console.WriteLine("아이템 번호를 선택하여 구매하거나 0을 눌러 나가세요.");
            BuyItem();
        }

        static void BuyItem() // 아이템 구매
        {
            string Input = Console.ReadLine();

            if (Input == "0")
            {
                Console.Clear();
                StartScene();
            }
            else
            {
                if (int.TryParse(Input, out int itemNumber) && itemNumber > 0 && itemNumber <= Item.storeItems.Count)
                {
                    Item store = new Item("dummy", 0, 0, 0, ItemType.Ect, ""); // 더미 객체 생성
                    store.BuyItem(itemNumber - 1);
                    Console.Clear();
                    Store();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    BuyItem();
                }
            }
        }

        static void Main(string[] args)
        {
            Item.AddStoreItems(); // 상점 아이템 설정
            StartScene(); // 시작 페이지 호출
        }
    }
}
