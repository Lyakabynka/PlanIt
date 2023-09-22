import { create } from "zustand";
import { IPlanGroup } from "../../entities/models/planGroup/planGroup";
import { $api, ENDPOINTS } from "../../shared";
import { ICreatePlanGroupRequest, IPlanPlanGroup, ISetPlanToPlanGroupRequestModel } from "../../entities";

interface IPlanGroupStore {
    isLoading: boolean

    planGroups: IPlanGroup[]

    getPlanGroups: () => Promise<void>

    createPlanGroup: (requestModel: ICreatePlanGroupRequest) => Promise<void>

    setPlansToPlanGroup: (id: string, requestModel: ISetPlanToPlanGroupRequestModel[]) => Promise<void>

    deletePlanGroup: (id: string) => Promise<void>

    // createScheduledPlanGroup: (requetModel: ICreate)
}

export const usePlanGroupStore = create<IPlanGroupStore>()((set, get) => ({
    isLoading: false,

    planGroups: [],

    getPlanGroups: async () => {
        set({ isLoading: true });

        const response = await $api.get(ENDPOINTS.PLANGROUP.GET_PLANGROUPS);

        if (response.status == 200) {
            const planGroups: IPlanGroup[] = response.data;

            set({ planGroups: planGroups });
        }

        set({ isLoading: false });
    },

    createPlanGroup: async (requestModel: ICreatePlanGroupRequest) => {
        set({ isLoading: true });

        const response = await $api.post<IPlanGroup>(ENDPOINTS.PLANGROUP.CREATE_PLANGROUP, requestModel);

        if (response.status === 200) {
            const createdPlanGroup = response.data;
            set({ planGroups: [...get().planGroups, createdPlanGroup] })
        }

        set({ isLoading: false });
    },

    setPlansToPlanGroup: async (id: string, requestModels: ISetPlanToPlanGroupRequestModel[]) => {
        set({ isLoading: true })

        const response = await $api.post(ENDPOINTS.PLANGROUP.SET_PLANS_TO_PLANGROUP.replace('{id:guid}', id), requestModels);

        set({ isLoading: false })
    },

    deletePlanGroup: async (id: string) => {
        set({ isLoading: true });

        const response = await $api.delete(ENDPOINTS.PLANGROUP.DELETE_PLANGROUP.replace('{id:guid}', id));

        set({ isLoading: false });
    },

    // createScheduledPlanGroup: async (requestModel: ) => {

    // }
}))