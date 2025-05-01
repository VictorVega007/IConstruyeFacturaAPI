using System.Security.Cryptography.X509Certificates;

namespace IConstruye.Factura.Core.Interfaces;

public interface ICertificateProvider
{
    X509Certificate2 GetDevelopmentCertificate();
}