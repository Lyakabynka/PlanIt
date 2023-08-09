import { createContext, useState } from "react";

const PagesContext = createContext();

export default PagesContext;

export const PagesProvider = ({children}) => {
    const [pages, setPages] = useState([]);
    
    return (
        <PagesContext.Provider value={{pages, setPages}}>
            {children}
        </PagesContext.Provider>
    )
}
