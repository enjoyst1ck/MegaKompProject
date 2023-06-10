export interface DeliveryMethod {
    sort(arg0: (a: any, b: any) => number): any;
    shortName: string;
    deliveryTime: string;
    description: string;
    price: number;
    id: number;
}