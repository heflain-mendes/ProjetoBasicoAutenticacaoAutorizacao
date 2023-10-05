using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Authorize.PolicyRequired;

/**
 * Essa classe é apenas um armazem de nome para as policies,
 * Com ela só é necessário mudar o nome da policie nesta classe
 * 
 * Futuramente podesse fazer com que os nome da policies fiquem atrelados ao appsettings.json
*/
public class Policies
{
    public const string HorarioComercial = nameof(HorarioComercial);
}
