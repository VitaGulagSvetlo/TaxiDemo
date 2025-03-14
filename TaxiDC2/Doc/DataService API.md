
# Dokumentace API – IDataService

**Rozhraní:** `TaxiDC2.Interfaces.IDataService`  
**Popis:** Rozhraní poskytující metody pro práci s daty taxi systému. Obsahuje operace pro správu jízd, zákazníků, řidičů, aut a nastavení.

---

## Obsah

1. [Přehled metod](#prehled-metod)
2. [Detailní popis metod](#detailni-popis-metod)
   - [AcceptTripByDriverAsync](#accepttripbydriverasync)
   - [FindCustomersAsync](#findcustomersasync)
   - [ForwardTripAsync](#forwardtripasync)
   - [GeocodeAsync (Obsolete)](#geocodeasync)
   - [GetCarByIdAsync](#getcarbyidasync)
   - [GetCarsAsync](#getcarsasync)
   - [GetClientMinVersionAsync](#getclientminversionasync)
   - [GetCustomerByIdAsync](#getcustomerbyidasync)
   - [GetCustomerByPhoneAsync](#getcustomerbyphoneasync)
   - [GetCustomersAsync](#getcustomersasync)
   - [GetDriverByIdAsync](#getdriverbyidasync)
   - [GetDriversAsync](#getdriversasync)
   - [GetServerVersionAsync](#getserverversionasync)
   - [GetTripAsync](#gettripasync)
   - [GetTripByIdAsync](#gettripbyidasync)
   - [ChangeTripStateAsync](#changetripstateasync)
   - [PingAsync](#pingasync)
   - [RegisterDriverAsync](#registerdriverasync)
   - [RejectTripAsync](#rejecttripasync)
   - [SaveCarAsync](#savecarasync)
   - [SaveCustomerAsync](#savecustomerasync)
   - [SaveDriverAsync](#savedriverasync)
   - [SaveTripAsync](#savetripasync)
   - [UpdateCustomerSettingsAsync](#updatecustomersettingsasync)
   - [UpdateDeviceKey](#updatedevicekey)
   - [UpdateDriverSettingsAsync](#updatedriversettingsasync)
3. [Použité typy](#pouzite-typy)
4. [Poznámky a doporučení](#poznamky-a-doporuceni)

---

## Přehled metod

Rozhraní **IDataService** definuje asynchronní operace, které zahrnují:

- **Operace s jízdami:** Akceptace, předání, změna stavu, získání jízdy/jízd.
- **Operace se zákazníky:** Hledání, získání podle ID/telefonu, uložení a aktualizace nastavení.
- **Operace s řidiči:** Registrace, získání řidiče či seznamu řidičů, odmítnutí jízdy.
- **Operace s auty:** Získání auta, seznam aut, uložení auta.
- **Systémové operace:** Kontrola serveru (ping), získání verze serveru a minimální verze klienta.

---

## Detailní popis metod

### AcceptTripByDriverAsync
**Účel:** Řidič akceptuje jízdu.  
**Parametry:**  
- `tripId` – identifikátor jízdy  
- `driverId` – identifikátor řidiče  
**Návratová hodnota:** `Task<bool>` – true, pokud byla akceptace úspěšná.

---

### FindCustomersAsync
**Účel:** Vrací seznam zákazníků podle zadaného filtru.  
**Parametry:**  
- `filter` – textový filtr pro vyhledávání  
**Návratová hodnota:** `Task<Customer[]>` – pole zákazníků splňujících kritéria.

---

### ForwardTripAsync
**Účel:** Předá jízdu jinému řidiči.  
**Parametry:**  
- `tripId` – identifikátor jízdy  
- `driverId` – identifikátor nového řidiče  
**Návratová hodnota:** `Task<bool>` – true, pokud bylo předání úspěšné.

---

### GeocodeAsync (Obsolete)
**Účel:** Získá geolokaci (pole `Lokace[]`) na základě textové adresy.  
**Parametry:**  
- `text` – textová adresa  
**Návratová hodnota:** `Task<Lokace[]>`  
**Poznámka:** Metoda je označena jako **Obsolete** (zastaralá).

---

### GetCarByIdAsync
**Účel:** Získá informace o autě podle ID.  
**Parametry:**  
- `carId` – identifikátor auta  
**Návratová hodnota:** `Task<Car>`

---

### GetCarsAsync
**Účel:** Vrací seznam aut.  
**Parametry:**  
- `activeOnly` – volitelný parametr (výchozí true) pro filtrování aktivních aut  
**Návratová hodnota:** `Task<Car[]>`

---

### GetClientMinVersionAsync
**Účel:** Získá minimální verzi klientské aplikace požadovanou serverem.  
**Návratová hodnota:** `Task<string>`

---

### GetCustomerByIdAsync
**Účel:** Získá zákazníka podle jeho ID.  
**Parametry:**  
- `customerId` – identifikátor zákazníka  
**Návratová hodnota:** `Task<Customer>`

---

### GetCustomerByPhoneAsync
**Účel:** Získá zákazníka podle telefonního čísla.  
**Parametry:**  
- `phone` – telefonní číslo zákazníka  
**Návratová hodnota:** `Task<Customer>`

---

### GetCustomersAsync
**Účel:** Vrací seznam zákazníků.  
**Parametry:**  
- `activeOnly` – volitelný parametr (výchozí true) pro filtrování aktivních zákazníků  
**Návratová hodnota:** `Task<Customer[]>`

---

### GetDriverByIdAsync
**Účel:** Získá řidiče podle jeho ID.  
**Parametry:**  
- `driverId` – identifikátor řidiče  
**Návratová hodnota:** `Task<Driver>`

---

### GetDriversAsync
**Účel:** Vrací seznam řidičů.  
**Parametry:**  
- `activeOnly` – volitelný parametr (výchozí true) pro filtrování aktivních řidičů  
**Návratová hodnota:** `Task<Driver[]>`

---

### GetServerVersionAsync
**Účel:** Vrací verzi serveru.  
**Návratová hodnota:** `Task<string>`

---

### GetTripAsync
**Účel:** Vrací seznam jízd.  
**Parametry:**  
- `activeOnly` – filtr pro aktivní jízdy  
**Návratová hodnota:** `Task<Trip[]>`

---

### GetTripByIdAsync
**Účel:** Získá konkrétní jízdu podle jejího ID.  
**Parametry:**  
- `tripId` – identifikátor jízdy  
**Návratová hodnota:** `Task<Trip>`

---

### ChangeTripStateAsync
**Účel:** Změní stav jízdy.  
**Parametry:**  
- `tripId` – identifikátor jízdy  
- `newState` – nový stav jízdy (typ `TripState`)  
- `paramsStrings` – další volitelné parametry (pole stringů)  
**Návratová hodnota:** `Task<bool>`

---

### PingAsync
**Účel:** Provádí kontrolu dostupnosti serveru.  
**Návratová hodnota:** `Task<bool>`

---

### RegisterDriverAsync
**Účel:** Registruje nového řidiče.  
**Parametry:**  
- `driver` – instance třídy `Driver`  
**Návratová hodnota:** `Task<bool>`

---

### RejectTripAsync
**Účel:** Řidič odmítá převzít jízdu.  
**Parametry:**  
- `tripId` – identifikátor jízdy  
- `driverId` – identifikátor řidiče  
**Návratová hodnota:** `Task<bool>`

---

### SaveCarAsync
**Účel:** Ukládá informace o autě.  
**Parametry:**  
- `car` – instance třídy `Car`  
**Návratová hodnota:** `Task<bool>`

---

### SaveCustomerAsync
**Účel:** Ukládá informace o zákazníkovi.  
**Parametry:**  
- `customer` – instance třídy `Customer`  
**Návratová hodnota:** `Task<bool>`

---

### SaveDriverAsync
**Účel:** Ukládá informace o řidiči.  
**Parametry:**  
- `driver` – instance třídy `Driver`  
**Návratová hodnota:** `Task<bool>`

---

### SaveTripAsync
**Účel:** Ukládá nebo aktualizuje jízdu.  
**Parametry:**  
- `trip` – instance třídy `Trip`  
**Návratová hodnota:** `Task<Trip>`

---

### UpdateCustomerSettingsAsync
**Účel:** Aktualizuje nastavení zákazníka.  
**Parametry:**  
- `customer` – instance třídy `Customer` s novými nastaveními  
**Návratová hodnota:** `Task<bool>`

---

### UpdateDeviceKey
**Účel:** Ukládá notifikační token zařízení k řidiči.  
**Parametry:**  
- `eToken` – token zařízení  
- `driverId` – identifikátor řidiče  
**Návratová hodnota:** `Task<bool>`

---

### UpdateDriverSettingsAsync
**Účel:** Aktualizuje nastavení řidiče.  
**Parametry:**  
- `driver` – instance třídy `Driver` s novými nastaveními  
**Návratová hodnota:** `Task<bool>`

---

## Použité typy

V dokumentaci se odkazují následující datové typy, jejichž definice nejsou součástí tohoto rozhraní, ale jsou předpokládány v rámci projektu:

- **Customer** – reprezentuje zákazníka.
- **Driver** – reprezentuje řidiče.
- **Car** – reprezentuje vozidlo.
- **Trip** – reprezentuje jízdu.
- **TripState** – výčet definující stavy jízdy.
- **Lokace** – reprezentuje geolokační údaje (používá se ve starší metodě GeocodeAsync).

---

## Poznámky a doporučení

- **Asynchronní operace:** Všechny metody pracují asynchronně a vracejí `Task` nebo `Task<T>`, což je vhodné pro zajištění nepřetržitého chodu aplikace (asynchronní I/O operace).
- **Obsolete metoda:** Metoda `GeocodeAsync` je označena atributem `[Obsolete]` a neměla by se dále používat. Mějte na paměti její případné odstranění v budoucích verzích API.
- **Verze API:** Metody `GetClientMinVersionAsync` a `GetServerVersionAsync` poskytují informace o verzích, což může být užitečné pro zajištění kompatibility mezi klientem a serverem.

---

## Závěr

Tato dokumentace shrnuje klíčové operace a účely metod definovaných v rozhraní **IDataService**. V případě potřeby rozšíření dokumentace (např. o příklady použití nebo diagramy) doporučujeme využít nástroje jako [Swagger](https://swagger.io/) či [Sandcastle](https://github.com/EWSoftware/SHFB) pro automatickou generaci dokumentace z kódu.

---

Tuto dokumentaci můžete uložit jako textový soubor (např. Markdown nebo HTML) a následně exportovat do PDF pomocí příslušných nástrojů nebo editorů.

Máte-li další dotazy nebo potřebujete podrobnější informace k některé metodě, dejte vědět!