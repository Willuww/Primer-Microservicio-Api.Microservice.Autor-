
//esta clase se encarga de el transporte de los datos del controlador hacia la logica de negocio
using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using FluentValidation;
using MediatR;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest { 
        public string Nombre { get; set; }

            public string Apellido { get; set; }

            public DateTime? FechaNacimiento { get; set; }
        }

        //clase para validar la clase ejecuta a traves del apifluent validator

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion() 
            { 
                RuleFor(p => p.Nombre).NotEmpty(); //No acepta valores nulos para la propiedad nombre
                RuleFor(p => p.Apellido).NotEmpty();
            }

            public class Manejador : IRequestHandler<Ejecuta>
            {
                public readonly ContextoAutor _context;

                public Manejador(ContextoAutor context)
                {
                    _context = context;
                }

                //aqui la tarea debe ser forzozamente asincrona

                public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    //se crea la instancia del autor-libro ligada al contexto
                    var autorLibro = new AutorLibro
                    {
                        Nombre = request.Nombre,
                        Apellido = request.Apellido,
                        FechaNacimiento = request.FechaNacimiento,
                        AutorLibroGuid = Convert.ToString(Guid.NewGuid())
                    };
                    //agregamos al objeto del tipo autor-libro
                    _context.AutorLibros.Add(autorLibro);
                        var respuesta = await _context.SaveChangesAsync();
                    if(respuesta > 0) 
                    {
                        return Unit.Value;
                    }
                    throw new Exception("No se pudo insertar el autor del libro");
                    
                }
            }
        }
    }
}
