using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using MyApp;



namespace Myapp

{
    
    
    // MODELOS
   
    class Producto
    {
public int Id { get; set; }
public string Nombre { get; set; }
public string Categoria { get; set; }
public decimal Precio { get; set; }
public int Stock { get; set; }
public bool EnOferta { get; set; }
public decimal PrecioFinal
        {
            get { return EnOferta ? Math.Round(Precio * 0.8m , 2) : Precio; }
        }
        }
        
        
        class ItemCarrito
    {
        public Producto Producto { get; set;}
        public int Cantidad { get; set; }
public int Subtotal { get { return Producto.PrecioFinal * Cantidad;} }
 }

 class Cliente
    {
        public string Nombre { get; set; }
        public decimal Dinero { get; set;}
        public int Puntos { get; set; }
        public List <ItemCarrito> Carrito { get; set; } = new List<ItemCarrito>();
        public string Deseo { get; set; }

    }


    // TIENDA

    class Tienda 
    {
    public List<Producto> Inventario { get; private set;}
    public Dictionary<string,decimal> Cupones { get; private set; }
    public decimal VentasTotales { get; set; }
    public int ClientesAtendidos { get; set; }
    private Random rnd = new Random();

    public Tienda()
    {
        CargarInventario();
        Cupones = new Dictionary<string, decimal>
        {
            { "TERRA" , 0.15m},
            { "WELCOME", 0.10m },
            { "SUPER20", 0.20m}
        };
        GenerarOfertas();
    }
    private void CargarInventario()
        {
            Inventario = new List<Producto>();
            int id = 1;
    Action<string, string, decimal, int> add = (n, c, p, s) =>
    {
        Inventario.Add(new Producto { Id = id++, Nombre = n, Categoria = c, Precio = p, Stock = s});
    };


    add("Manzana (Kg)","Frutas", 3.5m, 30);
    add("Banano (Kg)", "Frutas", 2.20m, 40);
    add("Mango", "Frutas", 1.80m, 25); 
    add("Fresas (caja)", "Frutas", 4.99m , 15);
    add("Tomate (Kg)", "Verduras", 3.10m , 30 );
    add("Lechuga", "verduras", 1.99m, 20);
    add("Zanahoria (Kg)", "Verduras", 2.00m, 25);
    add("Papa (Kg)", "Verduras",2.40m, 35 );
    add("Leche 1L", "Lacteos", 1.90m, 50);
    add("Queso Chedadar", "Lacteos", 5.75m, 20);
    add("Yogurt", "Lacteos", 1.10m, 40);
    add("Mantequilla", "Lacteos", 4.25m, 18);
    add("Pan Baguette", "Panaderia", 2.50m, 25);
    add("Croissant", "Panaderia", 1.75m, 30);
    add("Pastel de chocolate", "Panaderia", 12m, 6);
    add("Pollo (Kg)","Carnes", 8.99m, 20 );
    add("Carne molida (Kg)", "Carnes", 10.50m, 18);
    add("Salmon (Filete)", "Carnes", 14.99m, 10);
    add("Arroz 2Kg", "Despensa", 4.50m, 40);
    add("Pasta", "Despensa", 1.99m, 50);
    add("Aceite de Olivar", "Despensa", 9.99m, 15);
    add("Cade molido", " Despensa", 7.80m, 22);
    add("Maple syrup", " Despensa", 11.50m, 12);
    add("Refresco de 2L", "Bebidas", 2.99m, 40);
    add("Jugo de naranja", "Bebidas", 3.65m, 30);
    add("Agua (Pack de 12)", "Bebidas", 4.99m, 35);
    add("Papas fritas", "Snacks", 2.75m, 45);
    add("Chocolate", "Snacks", 1.99m, 60);
    add("Galletas", "Snacks", 2.30m, 50);
    add("Detergente", "Limpieza", 8.40m, 20);
    add("Papel higienico (Pack de 12)", "Limpieza", 9.20m, 25);
    add("Jamon (kg)", "carnes", 5.99m, 20);
    add("Carne Ahumada (Kg)", "Carnes", 8.99m, 20);
    add("Platos (6 pack)", "Cocina", 5.99m, 20);
    add("cuchillos (3 pack)", "Cocina", 4.99m, 40);
    add("Utensilios (12 pack)", "Cocina", 11.99m, 50);
    add("Gingibre (Kg)", "Verduras", 0.99m, 40);
    add("Frijole (Kg)", "Despensa", 4.99m, 50);
    add("Condimento de pollo", "Deespensa", 3.99m, 45);
    add("Maseca (1 Kg)", "Despensa", 9.99m, 20);
    add("Gansito", "Despensa", 0.99m, 100);
    add("Ganchos de pelo", "Belleza", 1.99m, 30);
    add("Cola elastica de pelo", "Belleza", 0.99m, 20);
    add("Peine", "Belleza", 1.49m, 30);
    add("Crema Cerave", "Belleza", 3.99m, 15);
    add("Salsa picante", "Despensa", 4.99m, 50);
    add("Espinaca", "Verduras", 1.99m, 60);
    add("Sal (Kg)", "Despensa", 4.99m, 30);
    add("Pimienta (Kg)", "Despensa", 4.90m, 50);
    add("Kiwi","Frutas", 3.99m, 30);
    
        }

        public void GenerarOfertas()
        {
            foreach (var p in Inventario) p.EnOferta = false;
for (int i = 0 ; i < 5; i++)
            {
                Inventario[ rnd.Next(Inventario.Count)].EnOferta = true;
            }
        }

public List<Producto> Buscar(string texto)
        {
            texto = (texto ?? "").ToLower();
            return Inventario.Where(p =>
            p.Nombre.ToLower().Contains(texto)||
            p.Categoria.ToLower().Contains(texto)).ToList();
        }
        }
}



// PROGRAMA/ INTERFAAZ


class program
{
    static Tienda tienda = new Tienda();
    static Cliente cliente;
    static decimal descuentoCupon = 0m;
    static string cuponUsado = "";
    static Random rnd = new Random();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Title = " Juego de supermercado";

        Pantalla();
        Console.WriteLine("==================================================================");
        Console.WriteLine(" WELCOME TO MY SHOP , I HOPE YOU ENJOY THIS experience :) ");
        Console.WriteLine("==================================================================");
        Console.WriteLine();

        bool SeguirJugando = true;
        while (SeguirJugando)
        {
            IniciarCliente();
            BucleCompra();
            tienda.ClientesAtendidos++;

            Console.WriteLine();
            Console.Write(" do you want to have another client? (y/n)");
            string r = (Console.ReadLine()??"").Trim().ToLower();
            SeguirJugando = r == "y" || r == "yes" || r == "Y" || r == "YES";
            if (SeguirJugando)
            {
                tienda.GenerarOfertas();
                descuentoCupon = 0m;
                cuponUsado = "";
            }


        }
        MostrarResumenFinal();

    }

static void Pantalla()
{
Console.Clear();
}
static void Pausa()
    {
        Console.WriteLine();
        Console.Write("Press ENTER to continue...");
        Console.ReadLine();
            }
//      INICIO DEL CLIENTE

static void IniciarCliente()
{
    Pantalla();
    Console.WriteLine(" Cashier: Hello, dear customer, could you please give me your name? ");
    Console.Write(">");
    string nombre = (Console.ReadLine()??"").Trim();
    if (nombre == "") nombre = "Cliente";
Console.WriteLine();
Console.WriteLine("Cashier: Nice to meet you" + nombre  + " What do you want to buy today?");
Console.Write(">");
string deseo = (Console.ReadLine()??"").Trim();
decimal dinero = Math.Round((decimal)(rnd.Next(4000, 12001)) /100m, 2)  ;
cliente = new Cliente {Nombre = nombre, Dinero = dinero, deseo = deseo }  ;
Console.WriteLine();
Console.WriteLine("Cashier: Perfect, you have $" + dinero.ToString("F2")+  "to spend");
if (deseo != "")
        {
            var encontorados = tienda.Buscar(deseo);
            if (encontorados.Count > 0)
            {
                Console.WriteLine ("Cashier: mmm, we have this related to" + deseo + ":");
                MostrarProductos(encontorados);

       }
            else
            {
                Console.WriteLine("mmmm, sorry we don't have " + deseo + "but feel free to look at everything else you might need" );

            }
        }
Pausa();

}

// bucle principal

static void BucleCompra()
    {
        Boolean enTienda = true;
        while (enTienda)
        {
            Pantalla();
            MostrarCabecera();
            Console.WriteLine("1. See full catalog");
            Console.WriteLine("2. Search a product");
            Console.WriteLine("3. Add to cart");
            Console.WriteLine("4. Remove from cart");
            Console.WriteLine("5. See cart");
            Console.WriteLine("6. Apply cupon");
            Console.WriteLine("7. Go to pay");
            Console.WriteLine("8. Minigame: give me my change");
            Console.WriteLine("9. See today offers");
            Console.WriteLine("0. Quit store");
            Console.Write("\n choose an option");
            string op = (Console.ReadLine()??"").Trim();

            switch (op)
            {
                case "1": VerCatalogo(); break;
                case "2": BuscarProducto(); break;
                case "3": AgregarAlCarrito(); break;
                case "4": QuitarDelCarrito(); break;
                case "5": VerCarrito(); break;
                case "6": AplicarCupon(); break;
                case "7":
                if (Pagar()) enTienda = false;
                break;
                case "8": MinijuegoCambio(); break;
                case "9": VerOfertas(); break;
                case "0": 
                Console.WriteLine("\n Cashier: See you next time" + cliente.Nombre + "!");
                Pausa();
                enTienda = false;
                break;
                default:
                Console.WriteLine("this option doesnt exist");
                Pausa();
                break;
            }
        
    





            }


        }
    

  static void MostrarCabecera()
{
    Console.WriteLine("=============================================================================================================");
    Console.WriteLine("Customer" + cliente.Nombre + " Dinero $" + cliente.Dinero.ToString("F2"))`;
    Console.WriteLine("Carrito" + cliente.Carrito.Sum(i => i.Cantidad) + "articulos    Total: $" + CalcularTotal().ToString("F2") );
    Console.WriteLine("Puntos de lealtad" + cliente.Puntos);
    Console.WriteLine("==============================================================================================================")
}

// CATALOGO
static void MostrarProductos(List<Producto> lista)
    {
        Console.WriteLine();
        Console.WriteLine("ID     |  Producto       |   Categoria   | Precio |  Stock");
        Console.WriteLine("-------+-----------------+---------------+--------+-------");
        foreach (var p in lista)
        {
            string precio = p.EnOferta
            ? "$" + p.PrecioFinal.ToString("F2") + "*"
            : "$"+ p.Precio.ToString("F2");
            Console.WriteLine(
                p.Id.ToString().padRight(3) + " | "  +
                p.Nombre.PadRight(25) +  " | " +
                p.Categoria.PadRight(11) + " | " +
                precio.PadRight(8) + " | " +
                p.Stock);

        }
        Console.WriteLine("\n (* = en oferrta, 20% de descuento)");

    }
static void VerCatalogo()
    {
        Pantalla();
        Console.WriteLine("=== CATALOGO===");
        foreach (var grupo in tienda.Inventario.GroupBy(p => p.Categoria))
        {
            Console.WriteLine("\n ---" + grupo.Key.ToUpper()+ "---");
            MostrarProductos(grupo.ToList());
        }
        Pausa();

    }
    static void BuscarProductos()
    {
        Console.WriteLine("\n que buscas?");
        string t = Console.ReadLine();
        var res = tienda.Buscar(t);
        if (res.Count == 0) Console.WriteLine("No se encontro nada");
        else MostrarProductos(res);
        Pausa();

    }
    static void VerOfertas()
    {
        Pantalla();
        Console.WriteLine("========== OFERTAS DEL DIA ==========");
        MostrarProductos(tienda.Inventario.Where(program => p.EnOferta).ToList());
        Pausa();
    }
    // CARRITOO
    static void AgregarAlCarrito()
    {
        Console.Write("\nID del producto a agregar");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("ID INVALID");
            Pausa();
            return;
        }
        var prod = tienda.Inventario.FirstOrDefault(p => p.Id == id);<
        if (prod == null)
        {
            Console.WriteLine("ESE PRODUCTO NO EXISTE");
            Pausa();
            return;

        }
        
    }
    
}