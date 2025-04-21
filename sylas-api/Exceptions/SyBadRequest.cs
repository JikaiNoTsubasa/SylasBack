using System;

namespace sylas_api.Exceptions;

public class SyBadRequest(string message) : SyException(message)
{
}
