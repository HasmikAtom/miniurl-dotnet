import React, {useState} from 'react';

type CellProps = {
    id: string
    alive: boolean
    activeCells: string[]
    setActiveCells:  React.Dispatch<React.SetStateAction<string[]>>
}

export const Cell: React.FC<CellProps> = ({id, activeCells, setActiveCells, alive}: CellProps) => {
    const [isClicked, setIsClicked] = useState(alive);

    const handleClick = () => {
        setIsClicked(!isClicked);
        if (!isClicked) {
           setActiveCells([...activeCells, id])
        } else {
            setActiveCells(activeCells.filter(cell => cell !== id))

        }

    };

    return (
        // <div id={id} className={`cell ${alive ? 'clicked-cell' : ''}`} onClick={handleClick}>
        // </div>
        <div id={id} className={`cell ${isClicked ? 'clicked-cell' : ''}`} onClick={handleClick}>
        </div>
    );
}