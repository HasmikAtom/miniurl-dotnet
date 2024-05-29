import './App.css'
import {Home} from "./components/Home.tsx"

function App() {

  return (
      <>
        <h1 className="text-3xl font-bold underline text-center">Game of Life</h1>
          <div className="h-screen flex items-center justify-center">
              <Home />
          </div>
      </>

  )
}

export default App
