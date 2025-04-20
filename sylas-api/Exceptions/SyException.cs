using System;

namespace sylas_api.Exceptions;

public class SyException(string message) : Exception(message)
{
}
