using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using static TextRPG.Program;
using System.Threading;


namespace TextRPG
{
    internal class Program
    {

        public class Player // 플레이어 클래스
        {
            public static int level = 1;
            public static string[] names = { "Chad" }; // 배열로 관리해보기
            public static string[] jobs = { "전사" };
            public static int attack = 10;
            public static int defense = 5;
            public static int hp = 100;
            public static int money = 1500;

        }


        public enum ItemType // 아이템 타입
        {
            Weapon,
            Armor,
            Ect
        }

        static List<Item> storeItems = new List<Item>()
            {
               // 아이템 이름      아이템 타입        아이템 스탯   아이템 설명      아이템 가격    아이템 수량
               // string itemName, ItemType itemType, int itemStat, string itemText, int itemPrice, int itemAmount
                
               new Item("수련자 갑옷", ItemType.Armor, 5, "수련에 도움을 주는 갑옷입니다", 1000, 1),
               new Item("무쇠갑옷", ItemType.Armor, 9, "무쇠로 만들어져 튼튼한 갑옷입니다", 1500, 1),
               new Item("스파르타의 갑옷", ItemType.Armor, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다", 10000, 1),
               new Item("낡은 검", ItemType.Weapon, 2, "쉽게 볼 수 있는 낡은 검입니다", 600, 1),
               new Item("청동 도끼", ItemType.Weapon, 5, "어디선가 사용됐던 거 같은 도끼입니다", 1500, 1),
               new Item("스파르타의 창", ItemType.Weapon, 7, "스파르타 전사들이 사용했다는 전설의 창입니다", 10000, 1),
            };

        static List<Item> inventoryItems = new List<Item>(); // 구입한 아이템을 담을 리스트

        public class Item // 아이템 클래스
        {
            public string itemName; // 아이템 이름
            public int itemPrice; // 아이템 가격
            public int itemAmount = 1; // 아이템 수량 1개로 지정
            public int itemStat;  // 아이템별 스탯
            public ItemType itemType; // 아이템 타입
            public string itemText;  // 아이템 각주
            public bool isEquipped; // 장착여부


            public Item(string itemName, ItemType itemType, int itemStat, string itemText, int itemPrice, int itemAmount) // 아이템 생성자
            {
                this.itemName = itemName;
                this.itemPrice = itemPrice;
                this.itemAmount = itemAmount;
                this.itemStat = itemStat;
                this.itemType = itemType;
                this.itemText = itemText;
                
            }

            public void AddStat() // 능력치 추가 함수
            {
                switch (itemType)
                {
                    case ItemType.Weapon: // 무기면 공격력 추가
                        Player.attack += itemStat;
                        break;
                    case ItemType.Armor: // 방어구면 방어력 추가
                        Player.defense += itemStat;
                        break;
                    case ItemType.Ect:
                        break;

                }
            }


            public void RevomeStat() // 능력치 제거 함수
            {
                switch (itemType)
                {
                    case ItemType.Weapon: // 무기면 공격력 추가
                        Player.attack -= itemStat;
                        break;
                    case ItemType.Armor: // 방어구면 방어력 추가
                        Player.defense -= itemStat;
                        break;
                    case ItemType.Ect:
                        break;

                }
            }

        }

        static void AddStoreItem(bool inStore) // 상점에 아이템을 추가함  // 상점 페이지 - 구매 페이지인지 구분하기 위해 bool로 조건
        {
           

            for (int i = 0; i < storeItems.Count; i++) 
            {
                var item = storeItems[i];
                string itemList = inStore? $"{i + 1}. {item.itemName}" : item.itemName;
                string priceInfo = "";

                if (inStore)
                {
                    if (item.itemAmount == 0)
                    {
                        priceInfo = "\t | [구매완료]";
                    }
                    else
                    {
                        priceInfo = $"\t | 가격: {item.itemPrice}G";
                    }
                }

                switch (item.itemType) 
                { 
                    case ItemType.Weapon:
                        Console.WriteLine($"{itemList} \t | 공격력 +{item.itemStat} \t | {item.itemText} {priceInfo}");
                        break;
                    case ItemType.Armor: // 방어구면 방어력 추가
                        Console.WriteLine($"{itemList} \t | 방어력 +{item.itemStat} \t | {item.itemText} {priceInfo}");
                        break;
                    case ItemType.Ect:
                        break;

                }
            }
        }

      
        public static void Buy(Item selectedItem) // 아이템 구입을 위한 함수
        {
            

            if (selectedItem.itemAmount == 0) // 이미 구매한 아이템
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                
            }
            else if (Player.money >= selectedItem.itemPrice)  // 돈이 충분할 때
            {
                selectedItem.itemAmount--;  // 수량을 빼준다
                Player.money -= selectedItem.itemPrice;
                inventoryItems.Add(selectedItem); // 구매한 아이템 리스트에 담는다
                Console.WriteLine("구매를 완료했습니다. 아이템 목록을 자동으로 업데이트합니다");
                Thread.Sleep(1500);
                Console.Clear();
                PurchaseScene();

            }
            else // 돈이 부족할 때
            {
                Console.WriteLine("Gold가 부족합니다");
                
            }
        }

       
        

        static void AddInventoryItem(bool inInventory) // 인벤토리에 넣을 아이템 리스트 만들기
        {
            if (inventoryItems.Count > 0)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    var item = inventoryItems[i];
                    string equippedStatus = item.isEquipped ? $"{i + 1}. [E]" : $"{i + 1}. "; // 장착되면 [E]가 보이게

                    switch (item.itemType)
                    {
                        case ItemType.Weapon:
                            Console.WriteLine($"{equippedStatus} {item.itemName} \t | 공격력 +{item.itemStat} \t | {item.itemText}");
                            break;
                        case ItemType.Armor: // 방어구면 방어력 추가
                            Console.WriteLine($"{equippedStatus} {item.itemName} \t | 방어력 +{item.itemStat} \t | {item.itemText}");
                            break;
                        case ItemType.Ect:
                            break;

                    }

                }
                
            }
            else
            {
                Console.WriteLine("보유한 아이템이 없습니다.");
            }
        }

        public static void EquippingItem(Item equippedItem) // 아이템 장착을 위한 함수
        {
            if (equippedItem.isEquipped) // 이미 장착한 아이템
            {
                Console.WriteLine("장착을 해제하겠습니까?");
                Console.WriteLine();
                Console.WriteLine("1. 장착 해제");
                Console.WriteLine("2. 장착 유지");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                while (true)   // 올바르게 입력할 때까지 실행되도록 
                {
                    String Input = Console.ReadLine();

                    if (Input == "1")
                    {
                        equippedItem.isEquipped = false;
                        equippedItem.RevomeStat(); // 플레이어의 스탯에 무기의 능력치를 제거한다
                        Console.WriteLine("장착을 해제했습니다. 잠시 후 상태창이 업데이트 됩니다");
                        Thread.Sleep(1500);
                        Console.Clear();
                        EquipItemScene();
                    }
                    else if (Input == "2")
                    {
                        Console.WriteLine("장착을 유지합니다");
                        Console.WriteLine();
                        Console.WriteLine("원하시는 행동을 입력해주세요.");
                        Console.WriteLine();
                        Console.WriteLine("1. 장착 해제");
                        Console.WriteLine("0. 나가기");

                        Console.Write(">>");

                        while (true) // 장착 해제 후 다시 장착을 할 수 있도록 기능 추가
                        {
                            String Input2 = Console.ReadLine();  // 콘솔창에 입력

                            if (Input2 == "0")
                            {
                                Console.Clear();
                                EquipItemScene(); // 시작 페이지 불러오기
                            }
                            else if(Input2 == "1")
                            {
                                equippedItem.isEquipped = false;
                                equippedItem.RevomeStat(); // 플레이어의 스탯에 무기의 능력치를 제거한다
                                Console.WriteLine("장착을 해제했습니다. 잠시 후 상태창이 업데이트 됩니다");
                                Thread.Sleep(1500);
                                Console.Clear();
                                EquipItemScene();
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                                Console.Write(">>");
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                        Console.Write(">>");
                    }
                }

            }
            else
            {
                equippedItem.isEquipped = true;
                equippedItem.AddStat(); // 플레이어의 스탯에 무기의 능력치를 추가한다
                Console.WriteLine("장착을 완료했습니다. 잠시 후 상태창이 업데이트 됩니다");
                Thread.Sleep(1500);
                Console.Clear();
                EquipItemScene();

            }
            
        }


        static void StartScene() // 시작 페이지
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");

            while (true)
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력해서 이동하기

                switch (Input)
                {
                    case "1": // 상태보기
                        Console.Clear();
                        StatusScene();
                        break;
                    case "2": // 인벤토리
                        Console.Clear();
                        InventoryScene();
                        break;
                    case "3": // 상점
                        Console.Clear();
                        StoreScene();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
                        break;
                }
            }
        }



        static void StatusScene() // 상태보기 페이지
        {
            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다");
            Console.WriteLine();
            Console.WriteLine("Lv. " + Player.level);
            Console.WriteLine($"{Player.names[0]} ({Player.jobs[0]})");
            Console.WriteLine("공격력 : " + Player.attack);
            Console.WriteLine("방어력 : " + Player.defense);
            Console.WriteLine("체력 : " + Player.hp);
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");


            while (true) // 올바르게 입력할 때까지 실행되도록 
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력

                if (Input == "0")
                {
                    Console.Clear();
                    StartScene(); // 시작 페이지 불러오기
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                    Console.Write(">>");
                }
            }
        }

        static void InventoryScene() // 인벤토리 페이지
        {
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            AddInventoryItem(false); // 구입한 아이템 리스트 생성

            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");

            


            while (true) // 올바르게 입력할 때까지 실행되도록 
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력

                if (Input == "0")
                {
                    Console.Clear();
                    StartScene(); // 시작 페이지 불러오기
                }
                if (Input == "1")
                {
                    Console.Clear();
                    EquipItemScene(); // 장착 관리 페이지 불러오기
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                    Console.Write(">>");
                }
            } 
        }

        static void EquipItemScene() // 인벤토리 - 장착 관리
        {
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            AddInventoryItem(true); // 인벤토리에 있는 아이템 리스트 생성
            
            Console.WriteLine();
            Console.WriteLine("장착/해제할 아이템 번호를 입력하세요");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");

            

            while (true) // 올바르게 입력할 때까지 실행되도록 
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력

                if (Input == "0")
                {
                    Console.Clear();
                    InventoryScene(); // 인벤토리 페이지 불러오기
                }
                else if (int.TryParse(Input, out int equippedItem) && equippedItem > 0 && equippedItem <= inventoryItems.Count)
                {
                    // 입력 받은 equippedItem를 아이템 장착하는 EquippingItem함수로 전달
                    EquippingItem(inventoryItems[equippedItem - 1]); // 구매한 아이템 리스트(inventoryItems)에서 선택한 인덱스의 아이템을 장착.

                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                    Console.Write(">>");
                }
            }
        }

        

        static void StoreScene() // 상점 페이지
        {
            
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다");
            Console.WriteLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine(Player.money +" G"); // 보유한 돈
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            AddStoreItem(false); // 아이템 리스트 표시 (인덱스 없음)


            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");


            while (true) // 올바르게 입력할 때까지 실행되도록 
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력

                if (Input == "0")
                {
                    Console.Clear();
                    StartScene(); // 시작 페이지 불러오기
                }
                if (Input == "1")
                {
                    Console.Clear();
                    PurchaseScene(); // 상품 구매 페이지 불러오기
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                    Console.Write(">>");
                }
            }

            
        }


        static void PurchaseScene() // 상점 - 물건 구입 페이지
        {
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다");
            Console.WriteLine();
            Console.WriteLine("[보유골드]");
            Console.WriteLine(Player.money + " G"); // 보유한 돈
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            AddStoreItem(true); // 아이템 리스트 표시 (인덱스 있음)
            
            Console.WriteLine();
            Console.WriteLine("구매할 아이템 번호를 입력하세요");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">>");




            while (true) // 올바르게 입력할 때까지 실행되도록 
            {
                String Input = Console.ReadLine();  // 콘솔창에 입력

                if (Input == "0")
                {
                    Console.Clear();
                    StoreScene(); // 인벤토리 페이지 불러오기
                    break;
                }
                else if (int.TryParse(Input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= storeItems.Count)
                {
                    // 입력 받은 selectedIndex를 Buy 함수로 전달
                    Buy(storeItems[selectedIndex - 1]); // storeItems 리스트에서 선택한 인덱스의 아이템을 구매
                   
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력하세요");
                    Console.Write(">>");
                }
            }
        }


        static void Main(string[] args)
        {
            StartScene(); 
        }
    }
}


// 작업하다가 궁금했던 점
    // 1. 클래스에 static 붙이는 이유    : 접근을 위해서로 이해했는데, 사용하다보니 언제 어떻게 사용해야할지 완벽하게 이해하지 못했음
    // 2. return의 용도와 목적 : 반환??  : 튜터님께 여쭤봤을 때, 클래스 안에 함수를 만들고 호출할 때 return.this;로 처리하면 된다고 하셨는데 return을 붙이다보니 오류가 생기는 경우가 있어 모두 제외해 봄