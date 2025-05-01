using System.Security.Cryptography.X509Certificates;

namespace IConstruye.Factura.Core.Interfaces;

public interface ICertificateProviderService
{
    X509Certificate2 GetDevelopmentCertificate();
}