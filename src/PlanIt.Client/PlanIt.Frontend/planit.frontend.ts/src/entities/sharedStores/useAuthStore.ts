import { EnumUserRole, ILoginRequest, IRegisterRequest, IUserData } from "../index";
import { create } from "zustand";
import { $api, decodeJwtToken } from "../../shared";
import ENDPOINTS from "../../shared/api/endpoints";
import { persist } from "zustand/middleware";
import { AES, enc } from 'crypto-js'

const encryptState = (state: any) => {
    const encryptedState = AES.encrypt(JSON.stringify(state), "secret-key-from-environment");
    return encryptedState.toString();
};

const decryptState = (encryptedState: any) => {
    const decryptedState = AES.decrypt(encryptedState, "secret-key-from-environment");
    return JSON.parse(decryptedState.toString(enc.Utf8));
};

interface IAuthStore {
    isLoggedIn: boolean;

    id: string | null;
    username: string | null;
    email: string | null;
    role: string | null;
    isEmailConfirmed: boolean | null;
    platform: string | null;

    isLoading: boolean;

    errorField: string | null;
    errorMessage: string | null;

    login: (params: ILoginRequest) => Promise<void>;
    register: (params: IRegisterRequest) => Promise<void>;
    logout: () => Promise<void>;

    clearAuth: () => void;
    resetErrorInfo: () => void;
}

export const useAuthStore = create<IAuthStore>()(persist((set, get) => ({
    isLoading: false,
    id: null,
    username: null,
    email: null,
    role: null,
    platform: null,
    isEmailConfirmed: null,
    isLoggedIn: false,
    errorField: null,
    errorMessage: null,

    login: async (params: ILoginRequest) => {

        const response = await $api.post<IUserData | any>(ENDPOINTS.AUTH.LOGIN, params); 
        console.log(response.data);
        

        if (response?.status == 401) {
            const error = response.data;
            set({ errorField: error.errorField, errorMessage: error.errorMessage })
        } else {

            console.log(response.data);

            const userData: IUserData = response.data;
            console.log(userData);

            set({
                id: userData?.id,
                username: userData?.username,
                email: userData?.email,
                role: userData?.role,
                isEmailConfirmed: userData?.isEmailConfirmed,
                platform: userData?.platform
            });

            set({ isLoggedIn: true });
        }

        set({ isLoading: false });
    },

    register: async (params: IRegisterRequest) => {

        set({ isLoading: true });

        const response = await $api.post<any>(ENDPOINTS.USER.REGISTER, params);

        if (response?.status == 409) {
            const error = response.data.error;
            set({ errorField: error.errorCode, errorMessage: error.message })
        }

        set({ isLoading: false });
    },

    logout: async () => {
        await $api.delete<any>(ENDPOINTS.AUTH.LOGOUT, {
            method: "DELETE"
        });

        const { clearAuth } = get();
        clearAuth();
    },

    clearAuth: () => {
        set({ role: null, isLoggedIn: false });
        localStorage.removeItem('auth');
    },
    resetErrorInfo: () => {
        set({ isLoading: false, errorField: null, errorMessage: null });
    }

}), {
    name: 'auth',
    version: 1,
    serialize: state => encryptState(state),
    deserialize: state => decryptState(state)
}));