import { useState } from 'react';
import './App.css';

interface AggregationOccurence {
    groupIdentifier: string;
    events: string[];
}

function App() {
    const [granularity, setGranularity] = useState<string>('');
    const [aggregations, setAggregations] = useState<AggregationOccurence[]>();
    const granularityOptions = [
        { value: 'MinuteByMinute', label: 'Minute by Minute' },
        { value: 'Hourly', label: 'Hourly' },
        { value: 'Daily', label: 'Daily' },
    ];

    const handleGranularityChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedValue = event.target.value;
        setGranularity(selectedValue);
        populateAggregationData(selectedValue);
    };

    const contents = aggregations === undefined
        ? <p><em>Please choose granularity.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
            <tr>
                <th>Time</th>
                <th>Events</th>
            </tr>
            </thead>
            <tbody>
            {aggregations.map((aggregation, i) => {
                return <tr key={i}>
                    <td>{aggregation.groupIdentifier}</td>
                    <td>{aggregation.events.map((event, ei) => {
                        return <p key={ei}>{event}</p>;
                    })}</td>
                </tr>
            })}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Chat Events</h1>
            <p>This component demonstrates fetching chat events from the server.</p>
            <div>
                <label htmlFor="selectBox">Choose granularity: </label>
                <select
                    id="selectBox"
                    value={granularity}
                    onChange={handleGranularityChange}
                >
                    <option value="" disabled>Select a level</option>
                    {granularityOptions.map((option) => (
                        <option key={option.value} value={option.value}>
                            {option.label}
                        </option>
                    ))}
                </select>
            </div>
            {contents}
        </div>
    );

    async function populateAggregationData(granularity: string) {
        if (!granularity) return;
        let roomId = 1;
        const response = await fetch(`api/v1/chat-rooms/${roomId}/aggregated-events/${granularity}`);
        if (!response.ok) {
            throw new Error(`Error: ${response.statusText}`);
        }
        const data = await response.json();
        setAggregations(data);
    }
}

export default App;