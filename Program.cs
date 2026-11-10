using System.Text;
using System.Xml.Serialization;


List<Partido> partidos = new List<Partido>();


using( StreamReader lector = new StreamReader("acb2526.csv", Encoding.UTF8)){

    lector.ReadLine();
    

    string? linea;
    while ((linea=lector.ReadLine()) != null)

    {
     string[]campos = linea.Split(','); //la coma actua de separador entre las columnas
     string[]resultado = campos[3].Split('-');   //el guion actua de separador entre los dos resultados

     Partido p = new Partido();

     p.Jornada = campos[0];
     p.EquipoLocal = campos[1];
     p.EquipoVisitante = campos[2];
     p.PuntosLocal = int.Parse(resultado[0]); //coge el campo del resultado, que se ha convertido en 2 numeros
     p.PuntosVisitante = int.Parse(resultado[1]);
     p.Mvp = campos[4];
     p.PuntosMvp = int.Parse(campos[5]); //Coge el campo directamente del csv
     p.AsistenciasMvp = int.Parse(campos[6]);
     p.RebotesMvp = int.Parse(campos[7]);
     p.EquipoMvp = campos[8];

     partidos.Add(p);  //Guarda el partido que he creado arriba

     
    }
    Console.WriteLine("Partidos Leidos : "+partidos.Count); //Cuenta cuantos partidos hay

    
}

int opcion = 0;  //Creo la variable para guardar la respuesta del usuario y pongo el 0 porque ninguna es valida
while (opcion != 6){  //Se hace el bucle pensando en cuando la respuesta del usuario sea 6 saldra

    Console.WriteLine("1. Resultados de un Equipo");
    Console.WriteLine("2. Datos de una Jornada");
    Console.WriteLine("3. Top 3 MVPS");
    Console.WriteLine("4. Clasificacion");
    Console.WriteLine("5. Guardar un fichero por equipo");
    Console.WriteLine("6. Salir");
    Console.Write("Elige una Opcion :");

    int.TryParse(Console.ReadLine(),out opcion); //convertimos la respuesta del usuario de un texto a un numero y si es una palabra se guarda el 0 en opcion
    

    switch (opcion){  //mira el valor de la opcion, y con el que haya sacado se va a su correspondiente case
        

        case 1: //resultados de un equipo
            {
                Console.WriteLine("Nombre del equipo : "); //El Usuario escribe el nombre del equipo
                string? equipo = Console.ReadLine();   //Lee el dato del usuario pudiendo ser null

                bool encontrado = false;  
                List<string> lineas = new List<string>(); //Una lista de textos para guardar cada linea para escribirlas luego en el fichero

                foreach (Partido p in partidos){ //recorre los partidos

                    if(p.EquipoLocal == equipo || p.EquipoVisitante == equipo){ //recorre el data set con el foreach de encima comparando si el nombre introducido corresponde con local o visitante 

                        string texto = p.Jornada+": "+p.EquipoLocal +" "+p.PuntosLocal+"-"+p.PuntosVisitante+" "+p.EquipoVisitante  //se crea texto de tipo string para guardar la linea de cada partido completa
                        +" MVP : "+ p.Mvp +"(" +p.PuntosMvp+ "PTS, "+p.AsistenciasMvp+" Assist, " + p.RebotesMvp + " REB, " + p.EquipoMvp+")";

                        Console.WriteLine(texto); //printea texto
                        lineas.Add(texto);        //Añade la linea al fichero

                        encontrado = true; //Cada vez que encuentre un partido el estado sera true para saber que habia resultados
                        
                    }
                    
                }
                if (!encontrado){

                    Console.WriteLine("No hay partidos de ese equipo");
                    
                }

                else{
                    Console.Write("Guardar en un fichero ? s/n : ");
                    string? respuesta = Console.ReadLine();

                    if (respuesta == "s"){

                        Console.Write("Nombre del fichero : ");
                        String? nombre = Console.ReadLine();

                        File.WriteAllLines(nombre+".txt",lineas);
                        Console.WriteLine("Fichero Guardado como "+nombre+".txt");
                        
                    }
                }

            }
            break;
        


        case 2:
            {
                
           
                Console.Write("Introduce el numero de Jornadas :");
                string? jornadaBuscada = Console.ReadLine();    // Lee la respuesta del usuario

                bool encontradaJornada = false;
                List<string> lineasJornadas =new List<string>(); //Crea una lista vacia

                foreach(Partido p in partidos){                 //lee los partidos

                    if(p.Jornada =="Jornada "+ jornadaBuscada)    //compara el jornada1 del csv para quedarnos solo con los partidos
                    {
                        string texto = p.Jornada + ": " + p.EquipoLocal + " " + p.PuntosLocal + "-" + p.PuntosVisitante + " " + p.EquipoVisitante
                        + " MVP : " + p.Mvp + " (" + p.PuntosMvp + " PTS, " + p.AsistenciasMvp + " Assist, " + p.RebotesMvp + " REB, " + p.EquipoMvp + ")";
                        //Monta en una sola variable de texto la línea completa de un partido

                        Console.WriteLine(texto);    //Printea la linea
                        lineasJornadas.Add(texto);   //Añade la linea al fichero
                        encontradaJornada = true;
                    }
                    
                } 

                if(!encontradaJornada)
                {
                    Console.WriteLine("No se encontraron partidos en esa jornada");
                }

                else{
                    Console.Write("Guardar en un fichero ? s/n : ");
                    string? respuestajorn = Console.ReadLine();

                    if(respuestajorn == "s")
                    {
                        Console.Write("Nombre del fichero : ");
                        string? nomjorn = Console.ReadLine();
                        File.WriteAllLines(nomjorn+".txt",lineasJornadas);
                        Console.WriteLine("Fichero Guardado como "+nomjorn+".txt");

                    }
                }
            }
            break;
        


        case 3:  //top 3 MVP
            {

            Dictionary<string, int> conteo = new Dictionary<string, int>(); //Utilizamos el dictionary en vez de list porque nos interesa guardar el nombre del jugador y las veces que ha sido MVP


            foreach(Partido p in partidos){

                    if (conteo.ContainsKey(p.Mvp))//Lee el diccionario y que estan todos apuntados
                    {
                        conteo[p.Mvp]++; //al haber pasado anteriormente por el bucle se le suma 1
                    }

                    else
                    {
                        conteo[p.Mvp] = 1; //si fuera la primera vez que pasa el jugador por aqui le dejaria en 1
                    }
                    
                }

                var top3 = conteo.OrderByDescending(jugador => jugador.Value).Take(3); //ordena los jugadores de mas a menos veces mvp y se queda con los 3 primeros 
                
                List<string> lineasMvp = new List<string>(); //Aqui guardamos las filas para el fichero
                int puesto = 1;   //contador para los jugadores

                foreach(var jugador in top3){

                    string texto = puesto + ". " + jugador.Key + " - " + jugador.Value + " veces MVP"; //aqui mostramos con key el nombre del jugador value las veces que ha sido mvp

                    Console.WriteLine(texto); //printea en consola
                    lineasMvp.Add(texto); //guarda para el fichero

                    puesto++;  //solo suma 1 para que pase al siguiente jugador
                    
                }

                Console.Write("Guardar en un fichero ? s/n : ");
                string? respuestaMvp = Console.ReadLine();       

                if (respuestaMvp == "s")                         
                {
                    Console.Write("Nombre del fichero : ");
                    string? nombreMvp = Console.ReadLine();      

                    File.WriteAllLines(nombreMvp + ".txt", lineasMvp);   
                    Console.WriteLine("Fichero guardado como " + nombreMvp + ".txt");
                }

            }



            break;



        case 4:  //Clasificacion
            {
                Dictionary<string, int> victorias = new Dictionary<string, int>();
                Dictionary<string, int> derrotas = new Dictionary<string, int>();

                foreach(Partido p in partidos)
                {   
                    if(!victorias.ContainsKey(p.EquipoLocal)){  //Al ser la primera vez/nuevo los apuntamos con 0 y 0
                        
                        victorias[p.EquipoLocal] = 0;
                        derrotas[p.EquipoLocal] = 0;
                    }

                    if (!victorias.ContainsKey(p.EquipoVisitante)){ //lo mismo que el anterior pero puede salir de visitante o local

                        victorias[p.EquipoVisitante] = 0;
                        derrotas[p.EquipoVisitante] = 0;
                    }

                    if (p.PuntosLocal > p.PuntosVisitante){ //si el local tiene mas puntos el local suma victoria y el rival suma derrota
        
                        victorias[p.EquipoLocal]++;
                        derrotas[p.EquipoVisitante]++;
                    }

                    else{                                   //y al reves 

                        victorias[p.EquipoVisitante]++;
                        derrotas[p.EquipoLocal]++;
                    }
                }

                var clasificacion = victorias.OrderByDescending(equipo => equipo.Value);
                // Ordena el diccionario de victorias de mayor a menor y lo guarda en clasificacion
                

                List<string> lineasClasif = new List<string>();   // aquí se guardan las filas para el fichero
                int puesto = 1;                                   // para numerar 1, 2, 3...

                foreach (var fila in clasificacion){
                     string texto = puesto + ". " + fila.Key + " - " + fila.Value + " V - " + derrotas[fila.Key] + " D";;    
                        //con el key sacamos el nombre del equipo, 
                    
                                           
                    Console.WriteLine(texto);                     
                    lineasClasif.Add(texto);                     
                    puesto++;                                     // le sumamos 1 para el siguiente equipo
                }
                Console.Write("Guardar en un fichero ? s/n : ");
                string? respuestaClasif = Console.ReadLine();     

                if (respuestaClasif == "s"){
                    Console.Write("Nombre del fichero : ");
                    string? nombreClasif = Console.ReadLine();    

                    File.WriteAllLines(nombreClasif + ".txt", lineasClasif);   
                    Console.WriteLine("Fichero guardado como " + nombreClasif + ".txt");
                }

            }


            break;

        case 5:  //Guardar un fichero
            List<string> equipos = new List<string>();

            foreach (Partido p in partidos){

                if (!equipos.Contains(p.EquipoLocal)){ //el equipo local no esta ya en la lista
                    equipos.Add(p.EquipoLocal);   //lo añade
                }

                if (!equipos.Contains(p.EquipoVisitante)){ //lo mismo
                    equipos.Add(p.EquipoVisitante);  //lo añade
                }
            }

            foreach (string equipo in equipos){    //recorre los equipos
                List<string> lineasEquipo = new List<string>();   // lista nueva, solo para este equipo

                foreach (Partido p in partidos){  //recorre los partidos
                    if (p.EquipoLocal == equipo || p.EquipoVisitante == equipo){ //y pregunta si de local o de visitante
                        
                        string texto = p.Jornada + ": " + p.EquipoLocal + " " + p.PuntosLocal + "-" + p.PuntosVisitante + " " + p.EquipoVisitante
                        + " MVP : " + p.Mvp + " (" + p.PuntosMvp + " PTS, " + p.AsistenciasMvp + " Assist, " + p.RebotesMvp + " REB, " + p.EquipoMvp + ")";
                        
                        lineasEquipo.Add(texto);

                    }
                }

                 File.WriteAllLines(equipo + ".txt", lineasEquipo); //esto creA un fichero con el nombre del equipo y escribe la linea por partido
            }
            
            Console.WriteLine("Se han creado " + equipos.Count + " ficheros");  //avisa de los ficheros que se han creado
            break;

        case 6: //Metodo Salir
         break;
        
        default:
            Console.WriteLine("Opcion invalida");       //Si la respuesta no es ninguno del 1 al 6 sera invalida
            break;
        
    }
}
