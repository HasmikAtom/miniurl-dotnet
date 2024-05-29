import React, {useState} from "react";
import { Button } from "@material-tailwind/react";
import {Cell} from "./Cell.tsx"
import {useGrid} from "../../services/game-service.ts";
import {useMutation} from "@tanstack/react-query";
import {startGameAPI} from "../../api/game-api.ts";
import {GameComponent} from "./GameComponent.tsx";

export const Grid: React.FC = () => {
    console.log("render")
    const { data: game, isLoading, isError} = useGrid()

    //replace this withuseMutation
    const [activeCells, setActiveCells] = useState<string[]>([])

    const startGame = useMutation({
        mutationFn: startGameAPI,
        onMutate: (newGame) => {
            console.log(newGame)
        },
        onSuccess: (newGame) => {
            return newGame
        }
    })



    // cosnt active = useMutation({
    //
    // })


    if (isLoading) return <p>Loading...</p>;
    if (isError) return <p>Error :(</p>;



        const cells = Array.from({ length: game?.length });
        const rows = Array.from({ length: game?.width }); // use memo here
        const activeC = game.activeCells
        // setActiveCells(game?.activeCells)

    return (

            <div style={{ display: 'flex', flexDirection: 'column' }}>
                <GameComponent></GameComponent>
                <Button
                    placeholder={""}
                    variant="filled"
                    loading={isLoading}
                    size="sm"
                    onClick={() => startGame.mutate({x: 1, y:1})}
                >
                    Start Game
                </Button>

                <div>{`grid size: ${game?.length} x ${game?.width}`}</div>
                {rows.map((_, rowIndex) => (
                    <div key={rowIndex} style={{ display: 'flex' }}>
                        {cells.map((_, cellIndex) => (
                            <Cell
                                id={`row-${rowIndex}_cell-${cellIndex}`}
                                key={`row-${rowIndex}_cell-${cellIndex}`}
                                activeCells={activeCells}
                                setActiveCells={setActiveCells}
                                alive={activeC.includes(`row-${rowIndex}_cell-${cellIndex}`)}
                                // alive={activeCells?.includes(`row-${rowIndex}_cell-${cellIndex}`)}
                            />
                        ))}
                    </div>
                ))}
            </div>
        );

}
