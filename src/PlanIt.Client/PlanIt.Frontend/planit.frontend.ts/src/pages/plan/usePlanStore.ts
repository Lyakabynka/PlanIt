import { persist } from "zustand/middleware";
import { ICreatePlanRequest, IPlan, ISchedulePlanRequest } from "../../entities";
import { create } from "zustand";
import { AES, enc } from "crypto-js";
import { $api, ENDPOINTS } from "../../shared";

interface IPlanStore {
    isLoading: boolean;
    plans: IPlan[]


    getPlans: () => Promise<void>;
    schedulePlan: (id: string, requestModel: ISchedulePlanRequest) => Promise<void>;
    deletePlan: (id: string) => Promise<void>;
    addPlan: (params: ICreatePlanRequest) => Promise<void>;
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

    schedulePlan: async (id: string, requestModel: ISchedulePlanRequest) => {
        set({ isLoading: true });

        console.log(id);
        console.log(requestModel);

        const response = await $api.post<any>(ENDPOINTS.PLAN.SCHEDULE_PLAN.replace('{id:guid}', id), requestModel);

        set({ isLoading: false });
    },

    deletePlan: async (id: string) => {
        await $api.delete(ENDPOINTS.PLAN.DELETE_PLAN.replace('{id:guid}', id));

        set({ plans: get().plans?.filter(plan => plan.id != id) });
    },

    addPlan: async (params: ICreatePlanRequest) => {

        set({isLoading: true});

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


        set({isLoading: false});
    }
}))