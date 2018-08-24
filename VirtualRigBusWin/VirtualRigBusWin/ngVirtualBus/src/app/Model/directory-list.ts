export class DirectoryList {
  rigBuses: Array<RigBusInfo>;
  dataBuses: Array<DataBusInfo>;
  docType: string;
  id: string;
  command: string;
  time: number;
  name: string;
  description: string;
  host: string;
  ip: null;
  udpPort: number;
  tcpPort: number;
  minVersion: number;
  maxVersion: number;
}

export class UdpCmdPacket {
    docType: string;
    id: string;
    command: string;
    time: number;
    name: string;
    description: string;
    host: string;
    ip: string;
    udpPort: number;
    tcpPort: number;
    minVersion: number;
    maxVersion: number;
}

export class RigBusInfo extends UdpCmdPacket {
    rigType: string;
    sendSyncInfo: string;
    honorTx: string;
    comPort: string;
}
export class DataBusInfo extends UdpCmdPacket {

}
