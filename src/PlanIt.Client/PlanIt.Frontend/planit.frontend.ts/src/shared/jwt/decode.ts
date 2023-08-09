// Function to decode JWT token
import jwtDecode from "jwt-decode";

interface IOriginDecodedJwtToken{
    jti: string;
    userId : number;
    role : string;
    exp : number;
}

export interface IDecodedJwtToken {
    jti: string;
    userId: number;
    userRole: string;
    expires: number;
}

export function decodeJwtToken(token: string): IDecodedJwtToken | null {
    try {
        console.log(token);
        
        const decodedObject = jwtDecode<IOriginDecodedJwtToken>(token);
        console.log(decodedObject);
        
        return {
            jti: decodedObject.jti,
            expires: decodedObject.exp,
            userId: decodedObject.userId,
            userRole: decodedObject.role
        }
    } catch (error) {
        console.error('Error decoding JWT token:', error);
        return null;
    }
}