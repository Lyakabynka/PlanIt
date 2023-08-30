import { create } from "zustand";
import { IPlanGroup } from "../../entities/models/planGroup/planGroup";
import { $api, ENDPOINTS } from "../../shared";

interface IPlanGroupStore {
    isLoading: boolean

    planGroups: IPlanGroup[]

    getGroups: () => Promise<void>


}

export const usePlanGroupStore = create<IPlanGroupStore>()((set, get) => ({
    isLoading: false,

    planGroups: [],

    getGroups: async () => {
        set({isLoading: true});

        const response = await $api.get(ENDPOINTS.PLANGROUP.GET_PLANGROUPS);
        
        if(response.status == 200)
        {
            const planGroups: IPlanGroup[] = response.data;

            set({planGroups: planGroups});
        }

        set({isLoading: false});
    },
    

}))