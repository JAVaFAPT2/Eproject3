import React from "react";
import { Box } from "@chakra-ui/react";
import ProductTable from "./components/ProductTable";
function ProductPage() {
    return (
        <>
            <Box pt={{ base: "130px", md: "70px", xl: "80px" }}>
                <ProductTable />        
            </Box>
        </>
    );
}
export default ProductPage;
