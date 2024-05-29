using Api.Microservice.Autor.Modelo;
using Api.Microservice.Autor.Persistencia;
using FluentValidation;
using MediatR;

namespace Api.Microservice.Autor.Aplicacion
{
    public class Nuevo
    {

        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }

            public string Apellido { get; }

            public DateTime? FechaNacimiento { get; set; }
        }
        //clase para validar la clase ejecuta a traves de apifluent validator
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(p => p.Nombre).NotEmpty();
                RuleFor(p => p.Apellido).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoAutor _context;
            public Manejador(ContextoAutor context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //se crea la instacia de autor-libro ligada al contexto
                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Convert.ToString(Guid.NewGuid())
                };
                //agregamos el objeto del tipo autor-libro
                _context.AutorLibros.Add(autorLibro);
                //insertamos el valor de insercion
                var respuesta = await _context.SaveChangesAsync();
                if (respuesta == 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el autor del libro");
            }
        }
    }
}
