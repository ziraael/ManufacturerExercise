export class OrderDTO {
    id?: number;
    customerId: string;
    orderDate: string;
    isReadyForCollection: boolean;
    isCanceled: boolean;
    isEngineProduced?:boolean;
    isChassisProduced?:boolean;
    isOptionPackProduced?:boolean;
    engineId:string;
    chassisId:string;
    optionPackId:string;
}