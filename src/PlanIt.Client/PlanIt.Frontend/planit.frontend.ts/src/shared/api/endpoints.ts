export const API_URL = 'http://localhost:5000';

export const ENDPOINTS = {
    AUTH : {
        LOGIN: `${API_URL}/auth/login`,
        REFRESH: `${API_URL}/auth/refresh`,
        LOGOUT: `${API_URL}/auth/logout`
    },
    USER :{
        REGISTER: `${API_URL}/user/register`,
        GET_PROFILE_DATA: `${API_URL}/user/profile`
    },
    ADMINISTRATOR: {
       
    },
    PLAN: {
        GET_PLANS: `${API_URL}/plans`,
        ADD_PLAN: `${API_URL}/plan`,
        DELETE_PLAN: `${API_URL}/plan/{id:guid}`
    },
    SCHEDULED_PLAN: {
        CREATE_SCHEDULED_PLAN: `${API_URL}/scheduled-plan`,
        DELETE_SCHEDULED_PLAN: `${API_URL}/scheduled-plan/{id:guid}`,
        GET_SCHEDULED_PLANS: `${API_URL}/scheduled-plans/{planId:guid}`
    },
    PLANGROUP: {
        GET_PLANGROUPS: `${API_URL}/plan-groups`,
        CREATE_PLANGROUP: `${API_URL}/plan-group`,
        SET_PLANS_TO_PLANGROUP: `${API_URL}/plan-group/{id:guid}/plans`,
        DELETE_PLANGROUP: `${API_URL}/plan-group/{id:guid}`,
        GET_PLANGROUP: `${API_URL}/plan-group/{id:guid}`
    },
    SCHEDULED_PLANGROUP: {
        CREATE_SCHEDULED_PLANGROUP: `${API_URL}/scheduled-plan-group`,
    }
}

export default ENDPOINTS;
