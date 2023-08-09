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
        SCHEDULE_PLAN: `${API_URL}/plan/{id:guid}/schedule`,
        ADD_PLAN: `${API_URL}/plan`,
        DELETE_PLAN: `${API_URL}/plan/{id:guid}`
    }
}

export default ENDPOINTS;
