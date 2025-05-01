using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IConstruye.Factura.Core.Interfaces;

namespace IConstruye.Factura.Core.Services;

public class CertificateProviderServiceService :  ICertificateProviderService
{
    private readonly X509Certificate2 _certificate;

    public CertificateProviderServiceService()
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest("CN=FakeDTECertificate", rsa, HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
        
        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false));
        
        _certificate =  request.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
    }
    
    public  X509Certificate2 GetDevelopmentCertificate() => _certificate;
}