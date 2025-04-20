using System;

namespace sylas_api.Exceptions;

public class SyEntitiyNotFoundException(string message) : SyException(message)
{
}
