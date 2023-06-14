import { Tooltip } from "reactstrap";
import { Area, AreaChart, CartesianGrid, Legend, Line, LineChart, ResponsiveContainer, XAxis, YAxis } from "recharts";


export function AreaChartWidget({ dayTemperatures }) {
    return (
        <>
        <ResponsiveContainer width="100%" height="100%" minWidth={100} minHeight={100}>
            <AreaChart
                width={200}
                height={50}
                data={dayTemperatures}
                margin={{
                    top: 0, right: 30, bottom: 0, left: -30,
                }}
            >
                <defs>
                    <linearGradient id="temperature" x1="0" y1="0" x2="0" y2="1">
                        <stop offset="10%" stopColor="red" />
                        <stop offset="40%" stopColor="orange" />
                        <stop offset="60%" stopColor="blue" />
                        <stop offset="80%" stopColor="navy" />
                    </linearGradient>
                </defs>
                <XAxis dataKey={"hour"} fontSize={"6px"} />
                <CartesianGrid stroke="#eee" strokeDasharray="10 10" />
                <YAxis fontSize={"6px"} />
                <Area
                    type="monotone"
                    dataKey="airTemperature"
                    stroke="#82ca9d"
                    activeDot={false}
                    fill="url(#temperature)" />
            </AreaChart>
        </ResponsiveContainer>
        </>
    )
}
