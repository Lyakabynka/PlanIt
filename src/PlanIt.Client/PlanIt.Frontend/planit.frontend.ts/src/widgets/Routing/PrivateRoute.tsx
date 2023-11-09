import {Navigate} from 'react-router-dom';
import {useAuthStore} from "../../entities";

export const PrivateRoute = ({children, requiredRole}: {
    children: JSX.Element;
    requiredRole: string;
}) => {
    const {isLoggedIn, isLoading, role, clearAuth} = useAuthStore();

    if (isLoading) {
        return <p className="container">Checking auth..</p>;
    }
    const userHasRequiredRole = requiredRole === role;

    if (!isLoggedIn || !userHasRequiredRole) {
        return <Navigate to='/'/>;
    }

    return children;
};