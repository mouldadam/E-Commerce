import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { API_BASE_URL } from "../../Utility/constants";

//Base Query with authentication
const baseQuery = fetchBaseQuery({
  baseUrl: API_BASE_URL + "/api",
});

const baseQueryWithAuth = async (args, _api, extraOptions) => {
  const result = await baseQuery(args, _api, extraOptions);

  return result;
};

export const baseApi = createApi({
    reducerPath:"Api",
    baseQuery: baseQueryWithAuth,
    tagTypes:[],
    endpoints: ()=>({}), //Endpoints defined in individual API files
})
