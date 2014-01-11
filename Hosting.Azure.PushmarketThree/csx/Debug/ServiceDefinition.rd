<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Hosting.Azure.PushmarketThree" generation="1" functional="0" release="0" Id="35f9f706-31c6-49e9-a7d6-5d24a71a0cae" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Hosting.Azure.PushmarketThreeGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ArtOfGroundFighting.Web:AdminEndpoint" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:AdminEndpoint" />
          </inToChannel>
        </inPort>
        <inPort name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
        <inPort name="ArtOfGroundFighting.Web:StorefrontEndpoint" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:StorefrontEndpoint" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="ArtOfGroundFighting.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapArtOfGroundFighting.WebInstances" />
          </maps>
        </aCS>
        <aCS name="Certificate|ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/MapCertificate|ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ArtOfGroundFighting.Web:AdminEndpoint">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/AdminEndpoint" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ArtOfGroundFighting.Web:StorefrontEndpoint">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/StorefrontEndpoint" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapArtOfGroundFighting.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.WebInstances" />
          </setting>
        </map>
        <map name="MapCertificate|ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ArtOfGroundFighting.Web" generation="1" functional="0" release="0" software="C:\DEV\Pushmarket\Code\Hosting.Azure.PushmarketThree\csx\Debug\roles\ArtOfGroundFighting.Web" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="AdminEndpoint" protocol="http" portRanges="8080" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="StorefrontEndpoint" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/SW:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ArtOfGroundFighting.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ArtOfGroundFighting.Web&quot;&gt;&lt;e name=&quot;AdminEndpoint&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;e name=&quot;StorefrontEndpoint&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.WebInstances" />
            <sCSPolicyUpdateDomainMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.WebUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ArtOfGroundFighting.WebUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ArtOfGroundFighting.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ArtOfGroundFighting.WebInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="c8814b6b-ab3e-4f96-a1d5-3115f80cde6c" ref="Microsoft.RedDog.Contract\ServiceContract\Hosting.Azure.PushmarketThreeContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="18450fb1-9575-4217-812c-6cfa74909666" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:AdminEndpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:AdminEndpoint" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="bfedffbf-1f15-44b1-8438-57537ec48f25" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="3cfd8696-9b57-4f90-9360-39d44e18e16f" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:StorefrontEndpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:StorefrontEndpoint" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>