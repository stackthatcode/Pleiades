<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Hosting.Azure.PushmarketThree" generation="1" functional="0" release="0" Id="12f10db5-6e6f-406c-98e2-a6a928152e7a" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="Hosting.Azure.PushmarketThreeGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ArtOfGroundFighting.Web:AogfEndpoint" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:AogfEndpoint" />
          </inToChannel>
        </inPort>
        <inPort name="ArtOfGroundFighting.Web:AogfEndpointAdministrative" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:AogfEndpointAdministrative" />
          </inToChannel>
        </inPort>
        <inPort name="ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/LB:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
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
        <lBChannel name="LB:ArtOfGroundFighting.Web:AogfEndpoint">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/AogfEndpoint" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ArtOfGroundFighting.Web:AogfEndpointAdministrative">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/AogfEndpointAdministrative" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
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
              <inPort name="AogfEndpoint" protocol="http" portRanges="80" />
              <inPort name="AogfEndpointAdministrative" protocol="http" portRanges="8080" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
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
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ArtOfGroundFighting.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ArtOfGroundFighting.Web&quot;&gt;&lt;e name=&quot;AogfEndpoint&quot; /&gt;&lt;e name=&quot;AogfEndpointAdministrative&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
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
    <implementation Id="22dcb072-47ae-4f98-a661-f7ea7d27d41a" ref="Microsoft.RedDog.Contract\ServiceContract\Hosting.Azure.PushmarketThreeContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="e0416662-5153-4dce-861b-d533d57f0c7a" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:AogfEndpoint@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:AogfEndpoint" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="e41bdc2f-9df1-428b-9b7d-929acda2927a" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:AogfEndpointAdministrative@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:AogfEndpointAdministrative" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="5a3b5e02-5d4a-4ae0-8783-4676b5499c87" ref="Microsoft.RedDog.Contract\Interface\ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/Hosting.Azure.PushmarketThree/Hosting.Azure.PushmarketThreeGroup/ArtOfGroundFighting.Web:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>