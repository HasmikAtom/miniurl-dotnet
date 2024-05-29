import {useQuery, useMutation} from "@tanstack/react-query";
import {fetchGameAPI, startGameAPI} from "../api/game-api.ts";

export const useGrid = () => {
    return useQuery({
        queryKey: ["game"],
        queryFn: fetchGameAPI
    });
}

export const useStartGame = () => {
    console.log("AAAAAAAA")

    return useMutation({
        mutationFn: startGameAPI,
        onMutate: (newGame) => {
            console.log(newGame)
        },
        onSuccess: (newGame) => {
            return newGame
        }
    })

}