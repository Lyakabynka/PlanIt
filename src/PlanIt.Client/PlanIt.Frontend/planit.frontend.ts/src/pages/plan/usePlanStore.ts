import {  ICreatePlanRequest, IPlan, ICreateScheduledPlanRequest, useAuthStore, IScheduledPlan } from "../../entities";
import { create } from "zustand";
import { $api, ENDPOINTS } from "../../shared";

interface IPlanStore {
    isLoading: boolean;
    plans: IPlan[]


    getPlans: () => Promise<void>;
    deletePlan: (id: string) => Promise<void>;
    addPlan: (params: ICreatePlanRequest) => Promise<void>;
    createScheduledPlan: (requestModel: ICreateScheduledPlanRequest) => Promise<void>;
    deleteScheduledPlan: (id: string) => Promise<void>;
}

export const usePlanStore = create<IPlanStore>()((set, get) => ({
    isLoading: false,
    plans: [],

    getPlans: async () => {

        set({ isLoading: true });

        const response = await $api.get<IPlan[]>(ENDPOINTS.PLAN.GET_PLANS);

        set({ plans: response.data, });

        set({ isLoading: false })
    },

    
    deletePlan: async (id: string) => {
        await $api.delete(ENDPOINTS.PLAN.DELETE_PLAN.replace('{id:guid}', id));
        
        set({ plans: get().plans?.filter(plan => plan.id != id) });
    },

    addPlan: async (params: ICreatePlanRequest) => {
        const response = await $api.post<IPlan>(ENDPOINTS.PLAN.ADD_PLAN, params);

        if(response.status === 400)
        {
            console.log(response);
        }
        else if(response.status === 200)
        {
            const createdPlan = response.data;
            set({plans: [...get().plans, createdPlan]})
        }
    },

    createScheduledPlan: async (requestModel: ICreateScheduledPlanRequest) => {
        //TODO: to prevent from re-loading Plans delete isLoading, because it makes render Loading element instead of plans
        // and add another loading into schedule plan dialog
        set({ isLoading: true });

        console.log(requestModel);

        const response = await $api.post<any>(ENDPOINTS.SCHEDULED_PLAN.CREATE_SCHEDULED_PLAN, requestModel);

        set({ isLoading: false });
    },

    deleteScheduledPlan: async (id: string) => {
        set({isLoading: true});

        const response = await $api.delete<any>(ENDPOINTS.SCHEDULED_PLAN.DELETE_SCHEDULED_PLAN.replace('{id:guid}', id));

        set({isLoading: false});
    },

    getScheduledPlans: async (planId: string) => {
        set({isLoading: true});

        const response = await $api.get<IScheduledPlan[]>(ENDPOINTS.SCHEDULED_PLAN.GET_SCHEDULED_PLANS.replace('{planId:guid}', planId));

        set({isLoading:false});
    }
}))