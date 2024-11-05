Herramientas usadas:

Visual Studio 2022 con .net 8 C# para el desarrollo.

microsoft sql management studio con sql express para la base de datos.

Swagger (ya integrado en el proyecto de la API) para hacer las pruebas.

Lucidchart para el diagrama de solucion.

Pasos para hacer las pruebas:

1- Haz click al botón de <> Codey selecciona abrir con Visual Studio o también puede descargarlo, descomprimir y luego abrirlo en Visual Studio.

2- Correr el script de creación BD en Sql Server Management Studio

3- Cambiar el valor del parámetro Data Source en la cadena de conexión, que se encuentra en el controlador CreacionUsuarioController.cs, por el server name del Sql Server Manament Studio instalado en la maquina donde se probara el proyecto.

4- Ejecuta el proyecto en Visual Studio con la opcion http del boton ejecutar, como se implemento Swagger el mismo se ejecutará desde ahí, donde se podrá hacer las pruebas de la API.

5- Ejecutar el método de login, el mismo cuenta con los datos de login de un usuario defecto, ya creado en la BD en el paso 2. Este devolverá un Jwt Token.

6- Ahora con el token obtenido en el paso anterior, puede ingresarlo haciendo click el boton authorize en Swagger, 
luego en el input ingrese la palabra Bearer seguido de un espacio y luego coloque el token. Luego de esto tendra autorizacion para crear un usuario en la BD, usando el Json que se encuentra en el Request body (puede modificarlos por los valores deseados). 
Luego de esto se mostrarán los datos del usuario registrado.
